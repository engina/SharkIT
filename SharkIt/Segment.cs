using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SharkIt
{
    public class Segment
    {
        Uri m_uri;
        CookieContainer m_cc;
        string m_key, m_header, m_body;
        byte[] m_buf = new byte[1024 * 2];
        TcpClient m_client;
        int m_start, m_end;
        Dictionary<string, string> m_headers = new Dictionary<string, string>();
        string m_path;
        FileStream m_fs;
        BinaryWriter m_bw;
        bool m_completed = false;
        long m_total = 0;
        long m_downloaded = 0;
        string m_filename;
        public delegate void HeadersReceivedHandler(object sender, Dictionary<string, string> headers);
        public event HeadersReceivedHandler HeadersReceived;
        public event EventHandler Completed;
        public event EventHandler Progress;

        public Segment(Uri uri, CookieContainer cc, string key, int start, int end, string path)
        {
            m_uri = uri;
            m_cc = cc;
            m_key = key;
            m_start = start;
            m_end = end;
            m_path = path;
            //Console.WriteLine("Starting segment " + start + "-" + end);
            m_filename = path + "_" + start.ToString("00000000") + "_" + end.ToString("00000000") + ".sharkit";
            //m_fs = new FileStream(m_filename, FileMode.Create, FileAccess.Write);
            //m_fs.Close();
            //File.SetAttributes(m_filename, FileAttributes.Hidden);
            m_fs = new FileStream(m_filename, FileMode.Create, FileAccess.Write);
            m_bw = new BinaryWriter(m_fs);
        }

        public string Filename
        {
            get
            {
                return m_filename;
            }
        }
        public bool IsCompleted
        {
            get
            {
                return m_completed;
            }
        }
        public void Start()
        {
            m_body = "streamKey=" + m_key;
            m_header = "POST " + m_uri.AbsolutePath + " HTTP/1.1\r\n" +
                        "Content-Type: application/x-www-form-urlencoded\r\n" +
                        "Host: " + m_uri.Host + "\r\n" +
                        "Cookie: " + m_cc.GetCookieHeader(m_uri) + "\r\n" +
                        "Content-Length: " + m_body.Length + "\r\n" +
                        "Expect: 100-continue\r\n" +
                        "Range: bytes=" + m_start + "-" + m_end + "\r\n" +
                        "Connection: Keep-Alive\r\n\r\n";
            Connect();
        }

        void Connect()
        {
            m_client = new TcpClient();
            m_client.BeginConnect(m_uri.Host, 80, new AsyncCallback(ConnectHandler), null);
        }
        void ConnectHandler(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            Socket sock = m_client.Client;
            sock.EndConnect(ar);
            if (!sock.Connected)
            {
                Console.WriteLine("Retrying " + m_path);
                Connect();
                return;
            }
            sock.Send(Encoding.ASCII.GetBytes(m_header));
            sock.BeginReceive(m_buf, 0, m_buf.Length, SocketFlags.None, new AsyncCallback(WaitContinueHandler), null);
        }

        void WaitContinueHandler(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            Socket sock = m_client.Client;

            int read = sock.EndReceive(ar);

            string response = Encoding.ASCII.GetString(m_buf);
            if (!response.StartsWith("HTTP/1.1 100 Continue\r\n\r\n"))
                throw new Exception("oops");
            sock.Send(Encoding.ASCII.GetBytes(m_body));
            sock.BeginReceive(m_buf, 0, m_buf.Length, SocketFlags.None, new AsyncCallback(WaitHeaderHandler), null);
        }

        void WaitHeaderHandler(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            Socket sock = m_client.Client;
            int read = sock.EndReceive(ar);
            string response = Encoding.ASCII.GetString(m_buf);
            string header = response.Substring(0, response.IndexOf("\r\n\r\n"));
            string[] t = header.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tokens = new List<string>(t);
            string respCode = tokens[0];

            if (!respCode.StartsWith("HTTP/1.1 206 Partial Content"))
                throw new Exception("wtf");

            tokens.RemoveAt(0);

            foreach (string token in tokens)
            {
                int delimeter = token.IndexOf(":");
                string key = token.Substring(0, delimeter);
                string val = token.Substring(delimeter + 1);
                m_headers[key] = val;
            }
            if (HeadersReceived != null)
                HeadersReceived(this, m_headers);
            m_downloaded = read - header.Length - 4;
            m_bw.Write(m_buf, header.Length + 4, read - header.Length - 4);
            sock.BeginReceive(m_buf, 0, m_buf.Length, SocketFlags.None, new AsyncCallback(WaitDataHandler), null);
        }

        public long Downloaded
        {
            get
            {
                return m_downloaded;
            }
        }

        void WaitDataHandler(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            Socket sock = m_client.Client;
            int read = sock.EndReceive(ar);
            if (read == 0)
            {
                m_total = m_fs.Position;
                if (m_total != (m_end - m_start))
                    Console.WriteLine("Incomplete download " + m_path);
                m_bw.Close();
                sock.Close();
                m_completed = true;
                if (Completed != null)
                    Completed(this, EventArgs.Empty);
                return;
            }
            m_bw.Write(m_buf, 0, read);
            m_downloaded += read;
            if (Progress != null)
                Progress(this, EventArgs.Empty);
            sock.BeginReceive(m_buf, 0, m_buf.Length, SocketFlags.None, new AsyncCallback(WaitDataHandler), null);
        }

        public override string ToString()
        {
            return "[Segment " + m_start + "-" + m_end + "]";
        }
    }
}
