using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharkIt.Properties;
using SharkIt.GrooveShark;

namespace SharkIt
{
    public partial class SharkIt : Form
    {
        private string m_path;
        private string m_clipboard;
        private Session m_session;
        private API m_api;

        public SharkIt()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            m_path = Properties.Settings.Default.Path;
            ShowInTaskbar = false;
            // Ignore about localization for now
            systemTray.Text = "Connecting...";
            //systemTray.Icon = Resources.download_red;
            systemTray.BalloonTipClicked += new EventHandler(systemTray_BalloonTipClicked);

            m_session = new Session();
            m_api = new API(m_session);
            m_session.GotSID += new Session.GotSIDHandler(m_gs_GotSID);
            m_session.GotToken += new Session.GotTokenHandler(m_gs_GotToken);

            Timer t = new Timer();
            t.Interval = 50;
            t.Tick += new EventHandler(ClipboardWatcher);
            t.Start();
        }

        void m_gs_GotToken(object sender, string token)
        {
            systemTray.Text = "Ready!";
            //systemTray.Icon = Resources.download_green;
        }

        void m_gs_GotSID(object sender, string sid)
        {
            systemTray.Text = "Right about there...";
            //systemTray.Icon = Resources.download_orange;
        }

        void ClipboardWatcher(object sender, EventArgs e)
        {
            if (Clipboard.GetText().Contains("listen.grooveshark.com/#/"))
            {
                if (m_clipboard != Clipboard.GetText())
                {
                    m_clipboard = Clipboard.GetText();
                    ClipBoardChanged(m_clipboard);
                }
            }
        }

        private void ClipBoardChanged(string clipboard)
        {
            if (clipboard.Contains("listen.grooveshark.com/#/playlist/"))
            {
                string[] tokens = clipboard.Split(new char[] { '/' });
                int playlistID = Int32.Parse(tokens[tokens.Length - 1]);
                DownloadPlaylist(playlistID);
            }
            else if (clipboard.Contains("listen.grooveshark.com/#/s/"))
            {
                string[] tokens = clipboard.Split(new char[] { '/' });
                string songToken = tokens[tokens.Length - 1];
                DownloadSong(songToken);
            }
            return;
        }

        private void DownloadPlaylist(int playlistID)
        {
        }

        private void DownloadSong(string songToken)
        {
        }

        void systemTray_BalloonTipClicked(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = m_path;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                m_path = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Path = m_path;
                Properties.Settings.Default.Save();
            }
        }

        private void clipboardWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Main().Show();
        }
    }
}
