namespace SharkIt
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.userNameTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.connectB = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.TextBox();
            this.playlistsCLB = new System.Windows.Forms.CheckedListBox();
            this.songsCLB = new System.Windows.Forms.CheckedListBox();
            this.downloadB = new System.Windows.Forms.Button();
            this.folderB = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.systemTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayCM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gotoFolderB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userNameTB
            // 
            this.userNameTB.Location = new System.Drawing.Point(5, 5);
            this.userNameTB.Name = "userNameTB";
            this.userNameTB.Size = new System.Drawing.Size(100, 20);
            this.userNameTB.TabIndex = 0;
            this.userNameTB.Text = "username";
            this.userNameTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.userNameTB_KeyUp);
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(111, 5);
            this.passTB.Name = "passTB";
            this.passTB.PasswordChar = '*';
            this.passTB.Size = new System.Drawing.Size(100, 20);
            this.passTB.TabIndex = 1;
            this.passTB.Text = "password";
            this.passTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // connectB
            // 
            this.connectB.Enabled = false;
            this.connectB.Location = new System.Drawing.Point(218, 4);
            this.connectB.Name = "connectB";
            this.connectB.Size = new System.Drawing.Size(142, 23);
            this.connectB.TabIndex = 2;
            this.connectB.Text = "please wait... connecting";
            this.connectB.UseVisualStyleBackColor = true;
            this.connectB.Click += new System.EventHandler(this.connectB_Click);
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(50, 697);
            this.logTB.Multiline = true;
            this.logTB.Name = "logTB";
            this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTB.Size = new System.Drawing.Size(809, 139);
            this.logTB.TabIndex = 3;
            this.logTB.Visible = false;
            // 
            // playlistsCLB
            // 
            this.playlistsCLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.playlistsCLB.FormattingEnabled = true;
            this.playlistsCLB.Location = new System.Drawing.Point(5, 30);
            this.playlistsCLB.Name = "playlistsCLB";
            this.playlistsCLB.Size = new System.Drawing.Size(149, 319);
            this.playlistsCLB.TabIndex = 4;
            this.playlistsCLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.playlsitsCLB_ItemCheck);
            // 
            // songsCLB
            // 
            this.songsCLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.songsCLB.FormattingEnabled = true;
            this.songsCLB.Location = new System.Drawing.Point(160, 30);
            this.songsCLB.Name = "songsCLB";
            this.songsCLB.Size = new System.Drawing.Size(496, 319);
            this.songsCLB.TabIndex = 5;
            // 
            // downloadB
            // 
            this.downloadB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadB.Location = new System.Drawing.Point(581, 4);
            this.downloadB.Name = "downloadB";
            this.downloadB.Size = new System.Drawing.Size(75, 23);
            this.downloadB.TabIndex = 6;
            this.downloadB.Text = "download";
            this.downloadB.UseVisualStyleBackColor = true;
            this.downloadB.Click += new System.EventHandler(this.downloadB_Click);
            // 
            // folderB
            // 
            this.folderB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.folderB.Location = new System.Drawing.Point(500, 4);
            this.folderB.Name = "folderB";
            this.folderB.Size = new System.Drawing.Size(75, 23);
            this.folderB.TabIndex = 7;
            this.folderB.Text = "save to";
            this.folderB.UseVisualStyleBackColor = true;
            this.folderB.Click += new System.EventHandler(this.folderB_Click);
            // 
            // systemTray
            // 
            this.systemTray.Text = "notifyIcon1";
            this.systemTray.Visible = true;
            // 
            // systemTrayCM
            // 
            this.systemTrayCM.Name = "systemTrayCM";
            this.systemTrayCM.Size = new System.Drawing.Size(61, 4);
            // 
            // gotoFolderB
            // 
            this.gotoFolderB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gotoFolderB.Location = new System.Drawing.Point(419, 4);
            this.gotoFolderB.Name = "gotoFolderB";
            this.gotoFolderB.Size = new System.Drawing.Size(75, 23);
            this.gotoFolderB.TabIndex = 8;
            this.gotoFolderB.Text = "go to folder";
            this.gotoFolderB.UseVisualStyleBackColor = true;
            this.gotoFolderB.Click += new System.EventHandler(this.gotoFolderB_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 352);
            this.Controls.Add(this.gotoFolderB);
            this.Controls.Add(this.userNameTB);
            this.Controls.Add(this.passTB);
            this.Controls.Add(this.songsCLB);
            this.Controls.Add(this.connectB);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.folderB);
            this.Controls.Add(this.downloadB);
            this.Controls.Add(this.playlistsCLB);
            this.MinimumSize = new System.Drawing.Size(550, 38);
            this.Name = "Main";
            this.Text = "SharkIT v0.6-beta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userNameTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.Button connectB;
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.CheckedListBox playlistsCLB;
        private System.Windows.Forms.CheckedListBox songsCLB;
        private System.Windows.Forms.Button downloadB;
        private System.Windows.Forms.Button folderB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.NotifyIcon systemTray;
        private System.Windows.Forms.ContextMenuStrip systemTrayCM;
        private System.Windows.Forms.Button gotoFolderB;
    }
}

