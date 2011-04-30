using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Net.Sockets;
using SharkIt.GrooveShark;
using System.Windows.Forms;
using System.Collections;

namespace SharkIt
{
    public class GS
    {
        public delegate void GotSIDHandler(object sender, string sid);
        public delegate void GotTokenHandler(object sender, string token);
        public delegate void LoggedInHandler(object sender, JObject user);
        public delegate void GotPlaylistsHandler(object sender, LinkedList<Playlist> playlists);
        public delegate void RequestSentHandler(object sender, string request);
        public delegate void DownloadProgressHandler(object sender, JObject song, long percentage);
        public delegate void SongListFetchedHandler(object sender, Playlist playlist);

        public event GotSIDHandler GotSID;
        public event GotTokenHandler GotToken;
        public event LoggedInHandler LoggedIn;
        public event GotPlaylistsHandler GotPlaylists;
        public event RequestSentHandler RequestSent;
        public event DownloadProgressHandler DownloadProgress;
        public event SongListFetchedHandler SongListFetched;

        private CookieContainer m_cc = new CookieContainer();
        private Uri m_uri = new Uri("http://grooveshark.com");
        private string m_sid = null;
        private string m_token = null;
        private string m_username;
        private string m_password;
        private JObject m_user;
        private LinkedList<Playlist> m_playlists;

        JObject m_countryObj = new JObject();
        
        public GS()
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
            tcp.BeginConnect("grooveshark.com", 80, new AsyncCallback(handlerSIDConnect), tcp);
        }

        private void handlerSIDConnect(IAsyncResult ar)
        {
            if (!ar.IsCompleted) return;
            TcpClient tcp = (TcpClient)ar.AsyncState;
            NetworkStream strm = tcp.GetStream();
            byte[] req = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: grooveshark.com\r\nConnection: Keep-Alive\r\n\r\n");
            strm.Write(req, 0, req.Length);
            byte[] buf = new byte[512];
            strm.BeginRead(buf, 0, 512, new AsyncCallback(handleSIDRead), new object[]{buf, strm});
        }

        private void handleSIDRead(IAsyncResult ar)
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
                    string[] cookies = header.Substring(12).Split(new char[]{';'});
                    foreach (string cookie in cookies)
                    {
                        string[] nameValue = cookie.Split(new char[] { '=' });
                        var name = nameValue[0];
                        var value = nameValue[1];
                        if (name == "PHPSESSID")
                        {
                            m_sid = value;
                            m_cc.Add(new Cookie(name, value, "/", "http://grooveshark.com"));
                            GotSID(this, m_sid);
                            GetToken();
                            strm.Close();
                        }
                    }
                }
            }
        }


        #region Public API

        public void Login(string username, string password)
        {
            NameValueCollection nvc = new NameValueCollection();
            m_username = username;
            m_password = password;
            nvc.Add("username", username);
            nvc.Add("password", password);
            CookieAwareWebClient wc = new CookieAwareWebClient(m_cc);
            wc.UploadValuesCompleted += new UploadValuesCompletedEventHandler(wc_UploadValuesCompleted);
            wc.UploadValuesAsync(new Uri("https://grooveshark.com/empty.php"), nvc);

        }

        public delegate void GetStreamKeyHandler(JObject song, string host, string streamKey, object state);

        public void GetStreamKey(JObject song, GetStreamKeyHandler handler, object state)
        {
            int id = Int32.Parse((string)song["SongID"]);
            JObject param = new JObject();
            param.Add("songID", id);
            param.Add("mobile", false);
            param.Add("prefetch", false);
            param.Add("country", m_countryObj);

            Request("getStreamKeyFromSongIDEx", param, new UploadStringCompletedEventHandler(getSongHandler), new object[]{ song, handler, state });
        }

        public void GetPlaylists()
        {
            JObject param = new JObject();
            param.Add("userID", m_user["userID"]);
            JObject headerOverride = new JObject();
            headerOverride.Add("client", "htmlshark");

            Request("userGetPlaylists", param, new UploadStringCompletedEventHandler(getPlaylistsHandler), null, headerOverride);
        }

        public void DownloadSong(string host, string key, JObject song)
        {
            if (song.ContainsKey("Downloaded"))
            {
                return;
            }
            CookieAwareWebClient wc = new CookieAwareWebClient(m_cc);
            //!FIXME Segmented downloads
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("streamKey", key);
            wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
            wc.UploadValuesCompleted += new UploadValuesCompletedEventHandler(wc_DownloadStream);
            wc.UploadValuesAsync(new Uri("http://"+host+"/stream.php"), "POST", nvc, song);
        }

        void wc_DownloadStream(object sender, UploadValuesCompletedEventArgs e)
        {
            JObject song = (JObject)e.UserState;
            string filename = (string)song["ArtistName"] + " - " + (string)song["Name"] + ".mp3";
            song["Downloaded"] = 100;
            string dst = "";
            string path = "";
            if (Main.PATH.Length != 0)
                path = Main.PATH + Path.DirectorySeparatorChar;
            dst = path + filename;
            FileStream fs = null;
            try
            {
                fs = new FileStream(dst, FileMode.Create);
            }
            catch (Exception ex)
            {
                char[] invalf = Path.GetInvalidFileNameChars();
                foreach (char c in invalf)
                    filename = filename.Replace(c, '_');
                dst = path + filename;
                try
                {
                    fs = new FileStream(dst, FileMode.Create);
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
                        fs = new FileStream(dst, FileMode.Create);
                    }
                    catch (Exception exc2)
                    {
                        MessageBox.Show("Could not save the file buddy. (" + exc2.Message + ")", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            fs.Write(e.Result, 0, e.Result.Length);
            fs.Close();
        }

        void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            JObject song = (JObject)e.UserState;
            long percentage = (e.BytesReceived * 100) / e.TotalBytesToReceive;
            song["Downloaded"] = percentage;
            DownloadProgress(this, song, percentage);
        }

        #endregion

        #region Handlers
        void getPlaylistsHandler(object sender, UploadStringCompletedEventArgs e)
        {
            m_playlists = new LinkedList<Playlist>();
            JObject response = (JObject) JSON.JsonDecode(e.Result);
            JObject result = (JObject) response["result"];
            ArrayList pl = (ArrayList)(result["Playlists"]);
            foreach (JObject t in pl)
            {
                Playlist p = new Playlist((JObject)t);
                m_playlists.AddLast(p);
                // Fetch songs of the all playlists in parallel
                _populatePlaylist(p);
            }
            GotPlaylists(this, m_playlists);
        }

        void _populatePlaylist(Playlist p)
        {
            JObject param = new JObject();
            param.Add("playlistID", p["PlaylistID"]);
            JObject headerOverride = new JObject();
            headerOverride.Add("client", "htmlshark");
            Request("playlistGetSongs", param, new UploadStringCompletedEventHandler(populatePlaylistHandler), (object)p, headerOverride);
        }

        private void populatePlaylistHandler(object sender, UploadStringCompletedEventArgs e)
        {
            Playlist p = (Playlist)e.UserState;
            JObject response = (JObject)JSON.JsonDecode(e.Result);
            JObject result = (JObject)response["result"];
            ArrayList songs = (ArrayList)result["Songs"];
            p["Songs"] = songs;
            SongListFetched(this, p);
        }

        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            JObject param = new JObject();
            param.Add("username", m_username);
            param.Add("password", m_password);
            param.Add("savePassword", 0);
            JObject header = new JObject();
            header.Add("client", "htmlshark");
            SecureRequest("authenticateUser", param, new UploadStringCompletedEventHandler(loginHandler), null, header);
        }

        private void loginHandler(object sender, UploadStringCompletedEventArgs e)
        {
            JObject response = (JObject)JSON.JsonDecode(e.Result);
            m_user = (JObject)response["result"];
            LoggedIn(this, m_user);
        }

        private void getUserByIdHandler(object sender, UploadStringCompletedEventArgs e)
        {
        }



        private void getSongHandler(object sender, UploadStringCompletedEventArgs e)
        {
            object[] state = (object[])e.UserState;
            JObject song = (JObject)state[0];
            GetStreamKeyHandler handler = (GetStreamKeyHandler)state[1];
            JObject result = (JObject)((JObject) JSON.JsonDecode(e.Result))["result"];
            handler(song, (string)result["ip"], (string)result["streamKey"], state[2]);
        }

        private void getPlayListHandler(object sender, UploadStringCompletedEventArgs e)
        {
            JObject result = (JObject)((JObject)JSON.JsonDecode(e.Result))["result"];
        }

        #endregion


        private JObject Request(string method, JObject parameters, UploadStringCompletedEventHandler handler, object handlerToken, JObject header)
        {
            return _Request("http://grooveshark.com/more.php?", method, parameters, handler, handlerToken, header);
        }

        private JObject SecureRequest(string method, JObject parameters, UploadStringCompletedEventHandler handler, object handlerToken, JObject header)
        {
            return _Request("https://cowbell.grooveshark.com/more.php?", method, parameters, handler, handlerToken, header);
        }

        private JObject Request(string method, JObject parameters, UploadStringCompletedEventHandler handler, object handlerToken)
        {
            return _Request("http://grooveshark.com/more.php?", method, parameters, handler, handlerToken, null);
        }

        private JObject SecureRequest(string method, JObject parameters, UploadStringCompletedEventHandler handler, object handlerToken)
        {
            return _Request("https://cowbell.grooveshark.com/more.php?", method, parameters, handler, handlerToken, null);
        }

        private JObject Request(string method, JObject parameters, UploadStringCompletedEventHandler handler)
        {
            return _Request("http://grooveshark.com/more.php?", method, parameters, handler, null, null);
        }

        private JObject SecureRequest(string method, JObject parameters, UploadStringCompletedEventHandler handler)
        {
            return _Request("https://cowbell.grooveshark.com/more.php?", method, parameters, handler, null, null);
        }

        private JObject _Request(string uri, string method, JObject parameters, UploadStringCompletedEventHandler handler, object handlerToken, JObject headerOverride)
        {
            string t = GenerateToken(method);
            JObject request = new JObject();
            request.Add("parameters", parameters);
            JObject header = new JObject();
            header.Add("token", t);
            header.Add("session", m_sid);
            header.Add("client", "jsqueue");
            header.Add("clientRevision", "20101012.37");
            header.Add("privacy", 0);
            // Somehow this uuid is important, and I don't really know what it is, the UUID of the JSQueue flash object ?
            header.Add("uuid", "6BFBFCDE-B44F-4EC5-AF69-76CCC4A2DAD0");
            header.Add("country", m_countryObj);
            request.Add("header", header);
            request.Add("method", method);

            if (headerOverride != null)
            {
                IDictionaryEnumerator e = headerOverride.GetEnumerator();
                while (e.MoveNext())
                {
                    if (header.ContainsKey(e.Key))
                        header[e.Key] = e.Value;
                    else
                        header.Add(e.Key, e.Value);
                }
            }
            string requestStr = JSON.JsonEncode(request).Replace("\n", "").Replace(" ", "").Replace("\r","");
            CookieAwareWebClient wc = new CookieAwareWebClient(m_cc);
            wc.UploadStringCompleted += handler;
            wc.UploadStringAsync(new Uri(uri + method), "POST", requestStr, handlerToken);
            RequestSent(this, requestStr);
            return request;
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

        private void GetToken()
        {
            CookieAwareWebClient wc = new CookieAwareWebClient(m_cc);
            string secret = MD5SUM(m_sid);
            string getCommunicationToken = "{\"parameters\":{\"secretKey\":\"" + secret + "\"},\"header\":{\"country\":{\"ID\":\"214\",\"CC1\":\"0\",\"CC2\":\"0\",\"CC3\":\"0\",\"IPR\":\"9581\",\"CC4\":\"2097152\"},\"privacy\":0,\"clientRevision\":\"20101222.03\",\"uuid\":\"6BFBFCDE-B44F-4EC5-AF69-76CCC4A2DAD0\",\"session\":\"" + m_sid + "\",\"client\":\"htmlshark\"},\"method\":\"getCommunicationToken\"}";
            wc.UploadStringCompleted += new UploadStringCompletedEventHandler(getTokenHandler);
            wc.UploadStringAsync(new Uri("http://grooveshark.com/more.php?getCommunicationToken"), getCommunicationToken);
        }

        void getTokenHandler(object sender, UploadStringCompletedEventArgs e)
        {
            JObject response = (JObject)JSON.JsonDecode(e.Result);
            string token = (string) response["result"];
            m_token = token;
            GotToken(this, token);
        }

        private string GetCookie(string key)
        {
            foreach(Cookie cookie in m_cc.GetCookies(m_uri))
                if (cookie.Name.Equals(key))
                    return cookie.Value;
            return null;
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
