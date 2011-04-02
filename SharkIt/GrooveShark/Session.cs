using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace SharkIt.GrooveShark
{
    public class Session
    {
        /**
         * Requests are either sent by HTML or Flash (JSqueue.swf) (I think)
         */
        public enum ClientType
        {
            HTML,
            JSQueue
        }

        private string m_sid;
        private string m_token;
        private Hashtable m_countryObj = new Hashtable();
        private bool m_ready = false;
        private CookieContainer m_cc = new CookieContainer();

        public delegate void GotSIDHandler(object sender, string sid);
        public delegate void GotTokenHandler(object sender, string token);
        public delegate void RequestSentHandler(object sender, string request);

        public delegate void RequestHandler(Session sender, JObject result, object state);
        public event GotSIDHandler GotSID;
        public event GotTokenHandler GotToken;
        public event RequestSentHandler RequestSent;

        public Session()
        {
            /** just create some default country for now **/
            m_countryObj.Add("CC4", "2097152");
            m_countryObj.Add("IPR", "9581");
            m_countryObj.Add("CC2", "0");
            m_countryObj.Add("CC1", "0");
            m_countryObj.Add("ID", "214");
            m_countryObj.Add("CC3", "0");

            /* I'm using TcpClient instead of WebClient here because I just want the cookie
             * in the HTTP header not the whole document. WebClient does not function before
             * it downloads whole document which takes 5 seconds on me. So this just gets the
             * first few bytes off the network and tries to parse the cookie asap.
             */
            TcpClient tcp = new TcpClient();
            tcp.BeginConnect("listen.grooveshark.com", 80, new AsyncCallback(HandlerSIDConnect), tcp);
        }

        public JObject Request(string method, JObject parameters, ClientType client, RequestHandler handler, object state)
        {
            return Request("http://listen.grooveshark.com/more.php", method, client, parameters, null, handler, state);
        }

        /**
         * This is the lowest level request method. Use higher level if possible. 
         */
        public JObject Request(string uri, string method, ClientType client, JObject parameters, JObject headerOverrides, RequestHandler handler, object state)
        {
            JObject request = new JObject();
            request.Add("parameters", parameters);
            JObject header = new JObject();
            if (m_token != null)
            {
                string t = GenerateToken(method);
                header.Add("token", t);
            }
            header.Add("session", m_sid);
            
            if (client == ClientType.HTML)
                header.Add("client", "htmlshark");
            else if (client == ClientType.JSQueue)
                header.Add("client", "jsqueue");
            else
                throw new Exception("ClientType not supported.");

            header.Add("clientRevision", "20101012.37");
            header.Add("privacy", 0);
            // Somehow this uuid is important, and I don't really know what it is, the UUID of the JSQueue flash object ?
            header.Add("uuid", "6BFBFCDE-B44F-4EC5-AF69-76CCC4A2DAD0");
            header.Add("country", m_countryObj);
            request.Add("header", header);
            request.Add("method", method);

            if (headerOverrides != null)
            {
                IDictionaryEnumerator e = headerOverrides.GetEnumerator();
                while (e.MoveNext())
                {
                    if (header.ContainsKey(e.Key))
                        header[e.Key] = e.Value;
                    else
                        header.Add(e.Key, e.Value);
                }
            }
            string requestStr = request.ToString().Replace("\n", "").Replace(" ", "").Replace("\r", "");
            CookieAwareWebClient wc = new CookieAwareWebClient(m_cc);
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(GSRequestHandler);
            wc.UploadStringAsync(new Uri(uri + "?" + method), "POST", requestStr, new object[]{ handler, state });
            if(RequestSent != null)
                RequestSent(this, requestStr);
            return request;
        }

        private void GSRequestHandler(object sender, UploadStringCompletedEventArgs args)
        {
            object[] state = (object[])args.UserState;
            RequestHandler handler = (RequestHandler)state[0];
            object calleeState = state[1];
            handler(this, (JObject)JSON.JsonDecode(args.Result), calleeState);
        }

        private string GenerateToken(string method)
        {
            m_lastRandomizer = Randomize();
            string r = SHA1(method + ":" + m_token + ":quitStealinMahShit:" + m_lastRandomizer);
            string t = m_lastRandomizer + r;
            return t;
        }

        private string m_lastRandomizer = "";

        private string Randomize()
        {
            string k = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
                k += random.Next(16).ToString("x");
            return k != m_lastRandomizer ? k : Randomize();
        }

        private void HandlerSIDConnect(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            TcpClient tcp = (TcpClient)ar.AsyncState;
            NetworkStream strm = tcp.GetStream();
            byte[] req = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: listen.grooveshark.com\r\nConnection: Keep-Alive\r\n\r\n");
            strm.Write(req, 0, req.Length);
            byte[] buf = new byte[512];
            strm.BeginRead(buf, 0, 512, new AsyncCallback(HandleSIDRead), new object[] { buf, strm });
        }

        private void HandleSIDRead(IAsyncResult ar)
        {
            object[] state = (object[])ar.AsyncState;
            byte[] buf = (byte[])state[0];
            NetworkStream strm = (NetworkStream)state[1];
            string response = System.Text.Encoding.ASCII.GetString(buf);
            string[] headers = response.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string header in headers)
            {
                if (header.StartsWith("Set-Cookie: "))
                {
                    string[] cookies = header.Substring(12).Split(new char[] { ';' });
                    foreach (string cookie in cookies)
                    {
                        string[] nameValue = cookie.Split(new char[] { '=' });
                        var name = nameValue[0];
                        var value = nameValue[1];
                        if (name == "PHPSESSID")
                        {
                            m_sid = value;
                            m_cc.Add(new Cookie(name, value, "/", "http://listen.grooveshark.com"));
                            if(GotSID != null)
                                GotSID(this, m_sid);
                            GetToken();
                            strm.Close();
                        }
                    }
                }
            }
        }

        private void GetToken()
        {
            JObject parameters = new JObject();
            parameters["secretKey"] =  MD5SUM(m_sid);
            Request("getCommunicationToken", parameters, ClientType.HTML, new RequestHandler(getCommunicationTokenHandler), null);
        }

        public void getCommunicationTokenHandler(Session gs, JObject response, object state)
        {
            string token = (string)response["result"];
            m_token = token;
            GotToken(this, token);
        }

        private string MD5SUM(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        private string SHA1(string Value)
        {
            System.Security.Cryptography.SHA1CryptoServiceProvider x = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        } 
    }
}
