namespace MouseFix
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
            this.Console = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timeUpdate = new System.Windows.Forms.Timer(this.components);
            this.contextWindowsStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.Multiline = true;
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(1446, 813);
            this.Console.TabIndex = 0;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Fixes mouse movement between screens with mixed DPI monitors.";
            this.notifyIcon.BalloonTipTitle = "Mouse Fix";
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Mouse Fix";
            this.notifyIcon.Visible = true;
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextWindowsStartup,
            this.contextExit});
            this.notifyIconContextMenu.Name = "notifyIconContextMenu";
            this.notifyIconContextMenu.Size = new System.Drawing.Size(301, 120);
            // 
            // contextExit
            // 
            this.contextExit.Name = "contextExit";
            this.contextExit.Size = new System.Drawing.Size(300, 36);
            this.contextExit.Text = "Exit";
            // 
            // timeUpdate
            // 
            this.timeUpdate.Interval = 20;
            this.timeUpdate.Tick += new System.EventHandler(this.timeUpdate_Tick);
            // 
            // contextWindowsStartup
            // 
            this.contextWindowsStartup.CheckOnClick = true;
            this.contextWindowsStartup.Name = "contextWindowsStartup";
            this.contextWindowsStartup.Size = new System.Drawing.Size(300, 36);
            this.contextWindowsStartup.Text = "Start With Windows";
            this.contextWindowsStartup.CheckedChanged += new System.EventHandler(this.contextWindowsStartup_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1446, 813);
            this.Controls.Add(this.Console);
            this.Name = "Main";
            this.Text = "Debug";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.notifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Console;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextExit;
        private System.Windows.Forms.Timer timeUpdate;
        private System.Windows.Forms.ToolStripMenuItem contextWindowsStartup;
    }
}

