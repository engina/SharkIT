using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SharkIt
{
    public class Download
    {
        Uri m_uri;
        string m_key;
        CookieContainer m_cc;
        string m_path;
        const int SEGMENT_SIZE = 512 * 1024;
        List<Segment> m_segments = new List<Segment>();
        DateTime m_start, m_end;
        int m_total = 0;
        JObject m_song;
        FileStream m_fs;

        public event EventHandler Progress;

        public Download(Uri uri, string key, CookieContainer cc, JObject song)
        {
            m_uri = uri;
            m_key = key;
            m_cc = cc;
            m_song = song;

            song["Status"] = "Opening file";
            string filename = (string)song["ArtistName"] + " - " + (string)song["Name"] + ".mp3";
            string dst = "";
            string path = "";
            if (Main.PATH.Length != 0)
                path = Main.PATH + Path.DirectorySeparatorChar;
            dst = path + filename;
            try
            {
                m_path = dst;
                m_fs = new FileStream(dst, FileMode.Create);
            }
            catch (Exception ex)
            {
                char[] invalf = Path.GetInvalidFileNameChars();
                foreach (char c in invalf)
                    filename = filename.Replace(c, '_');
                dst = path + filename;
                try
                {
                    m_path = dst;
                    m_fs = new FileStream(dst, FileMode.Create);
                }
                catch (Exception exc)
                {
                    for (int i = 0; i < dst.Length; i++)
                    {
                        if (!Char.IsLetterOrDigit(dst[i]))
                            filename = filename.Replace(dst[i], '_');
                        dst = path + filename;
                    }
                    try
                    {
                        m_path = dst;
                        m_fs = new FileStream(dst, FileMode.Create);
                    }
                    catch (Exception exc2)
                    {
                        throw new Exception("Could not save the file buddy. (" + exc2.Message + ")");
                    }
                }
            }

            song["Status"] = "Starting download";
            Segment s = new Segment(uri, cc, key, 0, SEGMENT_SIZE-1, m_path);
            m_segments.Add(s);
            s.Progress += new EventHandler(s_Progress);
            s.HeadersReceived += new Segment.HeadersReceivedHandler(s_HeadersReceived);
            s.Start();
            m_start = DateTime.Now;
        }

        public JObject Song
        {
            get
            {
                return m_song;
            }
        }

        void s_Progress(object sender, EventArgs e)
        {
            long t = 0;
            foreach (Segment s in m_segments)
                t += s.Downloaded;
            m_song["Status"] = "Downloading like a maniac";
            m_song["Downloaded"] = t;
            double elapsed = (DateTime.Now-m_start).TotalSeconds;
            m_song["Rate"] = t / elapsed / 1024;
            if (Progress != null)
                Progress(this, EventArgs.Empty);
        }

        void s_HeadersReceived(object sender, Dictionary<string, string> headers)
        {
            string range = headers["Content-Range"];
            m_total = Int32.Parse(range.Split('/')[1]);
            m_song["Total"] = m_total;
            m_song["Status"] = "Segmenting";
            int total = m_total - SEGMENT_SIZE;  // already downloaded
            int i = 0;
            while (total > 0)
            {
                i++;
                Segment s = new Segment(m_uri, m_cc, m_key, SEGMENT_SIZE * i, SEGMENT_SIZE * i + Math.Min(SEGMENT_SIZE, total) - 1, m_path);
                m_segments.Add(s);
                s.Completed += new EventHandler(s_Completed);
                s.Progress += new EventHandler(s_Progress);
                s.Start();
                total -= SEGMENT_SIZE;
            }
        }

        public long Downloaded
        {
            get
            {
                long t = 0;
                foreach (Segment s in m_segments)
                    t += s.Downloaded;
                return t;
            }
        }

        public long Total
        {
            get
            {
                return m_total;
            }
        }

        void s_Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Total segments: " + m_segments.Count);
            foreach (Segment s in m_segments)
            {
                if (!s.IsCompleted) return;
            }
            m_end = DateTime.Now;
            double elapsed = (m_end-m_start).TotalSeconds;
            Console.WriteLine("Completed in " + elapsed + " seconds " + (m_total/elapsed/1024) + " kb/sec");
            m_song["Status"] = "Done";
            m_song["Rate"] = 0.0d;
            foreach (Segment s in m_segments)
            {
                byte[] buf = new byte[32*1024];
                FileStream fs = new FileStream(s.Filename, FileMode.Open, FileAccess.Read);
                int read;
                while ((read = fs.Read(buf, 0, buf.Length)) > 0)
                    m_fs.Write(buf, 0, read);
                fs.Close();
                File.Delete(s.Filename);
            }
            m_fs.Close();
        }
    }
}
