using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.Threading;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using SharkIt.GrooveShark;

namespace SharkIt
{
    public partial class Main : Form
    {
        public static string PATH;

        private const string URI = "http://listen.grooveshark.com";
        GS m_gs;
        public Main()
        {
            InitializeComponent();
            PATH = Properties.Settings.Default.Path;
            userNameTB.Text = Properties.Settings.Default.Username;
            passTB.Text = Properties.Settings.Default.Password;
            m_gs = new GS();
            m_gs.RequestSent += new GS.RequestSentHandler(m_gs_RequestSent);
            m_gs.GotSID += new GS.GotSIDHandler(gs_GotSID);
            m_gs.GotToken += new GS.GotTokenHandler(gs_GotToken);
            m_gs.LoggedIn += new GS.LoggedInHandler(m_gs_LoggedIn);
            m_gs.GotPlaylists += new GS.GotPlaylistsHandler(m_gs_GotPlaylists);
            m_gs.DownloadProgress += new GS.DownloadProgressHandler(m_gs_DownloadProgress);
            m_gs.SongListFetched += new GS.SongListFetchedHandler(m_gs_SongListFetched);
            playlsitsCLB.SelectedValueChanged += new EventHandler(playlsitsCLB_SelectedValueChanged);
            songsCLB.Format += new ListControlConvertEventHandler(songsCLB_Format);
        }

        void m_gs_SongListFetched(object sender, Playlist playlist)
        {
            if (playlsitsCLB.SelectedItem == playlist)
                playlsitsCLB_SelectedValueChanged(playlsitsCLB, null);
        }

        void m_gs_DownloadProgress(object sender, JObject song, long percentage)
        {
            log("Downloading " + (string)song["Name"] + " " + percentage + "%");
            songsCLB.Invalidate();
        }

        void songsCLB_Format(object sender, ListControlConvertEventArgs e)
        {
            JObject song = (JObject)e.ListItem;
            if (song.Property("Empty") != null)
            {
                e.Value = "Loading...";
                return;
            }
            JProperty p = song.Property("Downloaded");
            string str = (string)song["ArtistName"] + " - " + (string)song["Name"] + ".mp3";
            if( p != null)
                str += " ("+(int)p.Value+"%)";
            e.Value = str;
        }
        private object m_lastSelected = null;
        void playlsitsCLB_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckedListBox lb = (CheckedListBox)sender;
            Playlist p = (Playlist)lb.SelectedItem;

            if (m_lastSelected == p) return;
            m_lastSelected = p;

            if (p == null) return;
            songsCLB.Items.Clear();
            if (p.Property("Songs") == null)
            {
                JObject loading = new JObject();
                loading["Empty"] = "true";
                songsCLB.Items.Add(loading);
                songsCLB.Enabled = false;
                return;
            }
            foreach (JToken t in p["Songs"])
            {
                songsCLB.Items.Add((JObject)t);
            }
            songsCLB.Enabled = true;
        }

        void m_gs_RequestSent(object sender, string request)
        {
            log("Request sent: " + request);
        }

        void m_gs_GotPlaylists(object sender, LinkedList<Playlist> playlists)
        {
            log("Playlists: " + playlists.ToString());
            foreach (Playlist p in playlists)
                playlsitsCLB.Items.Add(p);
            connectB.Text = "logged in";
        }

        void m_gs_LoggedIn(object sender, JObject user)
        {
            log("Logged in as: " + user.ToString());
            connectB.Text = "getting playlists";
            m_gs.GetPlaylists();
        }

        void m_gs_GotStream(JObject song, string ip, string key, object state)
        {
            log("Stream host: " + ip + " key: " + key);
            m_gs.DownloadSong(ip, key, song);
        }

        void gs_GotToken(object sender, string token)
        {
            log("Token: " + token);
            connectB.Enabled = true;
            connectB.Text = "log-in";
        }

        private delegate void GOTSIDDelegate(object sender, string e);

        void gs_GotSID(object sender, string e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new GOTSIDDelegate(gs_GotSID), new object[] { sender, e });
                return;
            }
            log("SID: " + e);
            connectB.Text = "SID received";
        }

        private void log(string str)
        {
            logTB.Text += str + "\r\n";
        }

        private void connectB_Click(object sender, EventArgs e)
        {
            if (!connectB.Enabled) return;
            m_gs.Login(userNameTB.Text, passTB.Text);
            Properties.Settings.Default.Username = userNameTB.Text;
            Properties.Settings.Default.Password = passTB.Text;
            Properties.Settings.Default.Save();
            connectB.Enabled = false;
            connectB.Text = "logging in...";
        }

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void downloadB_Click(object sender, EventArgs e)
        {
            foreach (object item in songsCLB.CheckedItems)
            {
                JObject song = (JObject)item;
                m_gs.GetStreamKey(song, new GS.GetStreamKeyHandler(m_gs_GotStream), null);
            }
        }

        private void folderB_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = PATH;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                PATH = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Path = PATH;
                Properties.Settings.Default.Save();
            }
        }

        private void passTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                connectB_Click(null, null);
        }

        private void userNameTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                connectB_Click(null, null);

        }

        private void playlsitsCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool check = e.NewValue == CheckState.Checked;
            for (int i = 0; i < songsCLB.Items.Count; i++)
                songsCLB.SetItemCheckState(i, e.NewValue);
        }
    }
}
