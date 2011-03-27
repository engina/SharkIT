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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.userNameTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.connectB = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.TextBox();
            this.playlsitsCLB = new System.Windows.Forms.CheckedListBox();
            this.songsCLB = new System.Windows.Forms.CheckedListBox();
            this.downloadB = new System.Windows.Forms.Button();
            this.folderB = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.systemTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayCM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabC = new System.Windows.Forms.TabControl();
            this.songSelectPage = new System.Windows.Forms.TabPage();
            this.downloadsPage = new System.Windows.Forms.TabPage();
            this.s = new System.Windows.Forms.CheckedListBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.tabC.SuspendLayout();
            this.songSelectPage.SuspendLayout();
            this.downloadsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // userNameTB
            // 
            this.userNameTB.Location = new System.Drawing.Point(6, 7);
            this.userNameTB.Name = "userNameTB";
            this.userNameTB.Size = new System.Drawing.Size(100, 20);
            this.userNameTB.TabIndex = 0;
            this.userNameTB.Text = "username";
            this.userNameTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.userNameTB_KeyUp);
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(112, 7);
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
            this.connectB.Location = new System.Drawing.Point(219, 6);
            this.connectB.Name = "connectB";
            this.connectB.Size = new System.Drawing.Size(142, 23);
            this.connectB.TabIndex = 2;
            this.connectB.Text = "please wait... connecting";
            this.connectB.UseVisualStyleBackColor = true;
            this.connectB.Click += new System.EventHandler(this.connectB_Click);
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(43, 354);
            this.logTB.Multiline = true;
            this.logTB.Name = "logTB";
            this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTB.Size = new System.Drawing.Size(809, 139);
            this.logTB.TabIndex = 3;
            this.logTB.Visible = false;
            // 
            // playlsitsCLB
            // 
            this.playlsitsCLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.playlsitsCLB.FormattingEnabled = true;
            this.playlsitsCLB.Location = new System.Drawing.Point(6, 32);
            this.playlsitsCLB.Name = "playlsitsCLB";
            this.playlsitsCLB.Size = new System.Drawing.Size(149, 499);
            this.playlsitsCLB.TabIndex = 4;
            this.playlsitsCLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.playlsitsCLB_ItemCheck);
            // 
            // songsCLB
            // 
            this.songsCLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.songsCLB.FormattingEnabled = true;
            this.songsCLB.Location = new System.Drawing.Point(161, 32);
            this.songsCLB.Name = "songsCLB";
            this.songsCLB.Size = new System.Drawing.Size(799, 499);
            this.songsCLB.TabIndex = 5;
            // 
            // downloadB
            // 
            this.downloadB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadB.Location = new System.Drawing.Point(885, 6);
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
            this.folderB.Location = new System.Drawing.Point(804, 6);
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
            // tabC
            // 
            this.tabC.Controls.Add(this.songSelectPage);
            this.tabC.Controls.Add(this.downloadsPage);
            this.tabC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabC.Location = new System.Drawing.Point(0, 0);
            this.tabC.Name = "tabC";
            this.tabC.SelectedIndex = 0;
            this.tabC.Size = new System.Drawing.Size(974, 565);
            this.tabC.TabIndex = 8;
            // 
            // songSelectPage
            // 
            this.songSelectPage.Controls.Add(this.userNameTB);
            this.songSelectPage.Controls.Add(this.passTB);
            this.songSelectPage.Controls.Add(this.connectB);
            this.songSelectPage.Controls.Add(this.folderB);
            this.songSelectPage.Controls.Add(this.playlsitsCLB);
            this.songSelectPage.Controls.Add(this.downloadB);
            this.songSelectPage.Controls.Add(this.songsCLB);
            this.songSelectPage.Controls.Add(this.logTB);
            this.songSelectPage.Location = new System.Drawing.Point(4, 22);
            this.songSelectPage.Name = "songSelectPage";
            this.songSelectPage.Padding = new System.Windows.Forms.Padding(3);
            this.songSelectPage.Size = new System.Drawing.Size(966, 539);
            this.songSelectPage.TabIndex = 0;
            this.songSelectPage.Text = "Song select";
            this.songSelectPage.UseVisualStyleBackColor = true;
            // 
            // downloadsPage
            // 
            this.downloadsPage.Controls.Add(this.numericUpDown1);
            this.downloadsPage.Controls.Add(this.s);
            this.downloadsPage.Location = new System.Drawing.Point(4, 22);
            this.downloadsPage.Name = "downloadsPage";
            this.downloadsPage.Padding = new System.Windows.Forms.Padding(3);
            this.downloadsPage.Size = new System.Drawing.Size(966, 539);
            this.downloadsPage.TabIndex = 1;
            this.downloadsPage.Text = "Downloads";
            this.downloadsPage.UseVisualStyleBackColor = true;
            // 
            // s
            // 
            this.s.FormattingEnabled = true;
            this.s.Location = new System.Drawing.Point(4, 4);
            this.s.Name = "s";
            this.s.Size = new System.Drawing.Size(954, 499);
            this.s.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(68, 513);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 565);
            this.Controls.Add(this.tabC);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "SharkIT";
            this.tabC.ResumeLayout(false);
            this.songSelectPage.ResumeLayout(false);
            this.songSelectPage.PerformLayout();
            this.downloadsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox userNameTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.Button connectB;
        private System.Windows.Forms.TextBox logTB;
        private System.Windows.Forms.CheckedListBox playlsitsCLB;
        private System.Windows.Forms.CheckedListBox songsCLB;
        private System.Windows.Forms.Button downloadB;
        private System.Windows.Forms.Button folderB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.NotifyIcon systemTray;
        private System.Windows.Forms.ContextMenuStrip systemTrayCM;
        private System.Windows.Forms.TabControl tabC;
        private System.Windows.Forms.TabPage songSelectPage;
        private System.Windows.Forms.TabPage downloadsPage;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckedListBox s;
    }
}

