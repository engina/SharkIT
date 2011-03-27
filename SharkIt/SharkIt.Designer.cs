namespace SharkIt
{
    partial class SharkIt
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
            this.systemTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayCM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clipboardWatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.systemTrayCM.SuspendLayout();
            this.SuspendLayout();
            // 
            // systemTray
            // 
            this.systemTray.ContextMenuStrip = this.systemTrayCM;
            this.systemTray.Text = "SharkIt!";
            this.systemTray.Visible = true;
            // 
            // systemTrayCM
            // 
            this.systemTrayCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.clipboardWatchToolStripMenuItem,
            this.saveFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.systemTrayCM.Name = "systemTrayCM";
            this.systemTrayCM.Size = new System.Drawing.Size(154, 120);
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.mainToolStripMenuItem.Text = "Main";
            this.mainToolStripMenuItem.Click += new System.EventHandler(this.mainToolStripMenuItem_Click);
            // 
            // clipboardWatchToolStripMenuItem
            // 
            this.clipboardWatchToolStripMenuItem.Name = "clipboardWatchToolStripMenuItem";
            this.clipboardWatchToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.clipboardWatchToolStripMenuItem.Text = "Clipboard Watch";
            this.clipboardWatchToolStripMenuItem.Click += new System.EventHandler(this.clipboardWatchToolStripMenuItem_Click);
            // 
            // saveFolderToolStripMenuItem
            // 
            this.saveFolderToolStripMenuItem.Name = "saveFolderToolStripMenuItem";
            this.saveFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveFolderToolStripMenuItem.Text = "Settings";
            this.saveFolderToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // SharkIt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "SharkIt";
            this.Text = "SharkIt";
            this.systemTrayCM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon systemTray;
        private System.Windows.Forms.ContextMenuStrip systemTrayCM;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clipboardWatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}