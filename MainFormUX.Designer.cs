namespace Akuma
{
    partial class MainFormUX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormUX));
            this.lblTime = new System.Windows.Forms.Label();
            this.txtDo = new System.Windows.Forms.TextBox();
            this.timex = new System.Windows.Forms.Timer(this.components);
            this.lnkExit = new System.Windows.Forms.LinkLabel();
            this.lnkTimesheet = new System.Windows.Forms.LinkLabel();
            this.lnkSt = new System.Windows.Forms.LinkLabel();
            this.dgvTimeSheet = new System.Windows.Forms.DataGridView();
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.generateReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeSheet)).BeginInit();
            this.dgvContextMenu.SuspendLayout();
            this.formContextMenu.SuspendLayout();
            this.trayContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(28, 28);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 17);
            this.lblTime.TabIndex = 0;
            // 
            // txtDo
            // 
            this.txtDo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDo.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDo.ForeColor = System.Drawing.Color.DarkGray;
            this.txtDo.Location = new System.Drawing.Point(12, 116);
            this.txtDo.Name = "txtDo";
            this.txtDo.Size = new System.Drawing.Size(145, 23);
            this.txtDo.TabIndex = 1;
            this.txtDo.Text = "What you are doing ?";
            this.txtDo.Leave += new System.EventHandler(this.txtDo_Leave);
            this.txtDo.Enter += new System.EventHandler(this.txtDo_Enter);
            // 
            // timex
            // 
            this.timex.Tick += new System.EventHandler(this.timex_Tick);
            // 
            // lnkExit
            // 
            this.lnkExit.AutoSize = true;
            this.lnkExit.BackColor = System.Drawing.Color.Transparent;
            this.lnkExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkExit.ForeColor = System.Drawing.Color.Transparent;
            this.lnkExit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkExit.LinkColor = System.Drawing.Color.Red;
            this.lnkExit.Location = new System.Drawing.Point(133, 227);
            this.lnkExit.Name = "lnkExit";
            this.lnkExit.Size = new System.Drawing.Size(25, 13);
            this.lnkExit.TabIndex = 3;
            this.lnkExit.TabStop = true;
            this.lnkExit.Text = "Exit";
            this.lnkExit.VisitedLinkColor = System.Drawing.Color.Red;
            this.lnkExit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExit_LinkClicked);
            // 
            // lnkTimesheet
            // 
            this.lnkTimesheet.AutoSize = true;
            this.lnkTimesheet.BackColor = System.Drawing.Color.Transparent;
            this.lnkTimesheet.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTimesheet.ForeColor = System.Drawing.Color.Transparent;
            this.lnkTimesheet.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkTimesheet.LinkColor = System.Drawing.Color.Red;
            this.lnkTimesheet.Location = new System.Drawing.Point(9, 227);
            this.lnkTimesheet.Name = "lnkTimesheet";
            this.lnkTimesheet.Size = new System.Drawing.Size(100, 13);
            this.lnkTimesheet.TabIndex = 4;
            this.lnkTimesheet.TabStop = true;
            this.lnkTimesheet.Text = "Expand TimeSheet";
            this.lnkTimesheet.VisitedLinkColor = System.Drawing.Color.Red;
            this.lnkTimesheet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTimesheet_LinkClicked);
            // 
            // lnkSt
            // 
            this.lnkSt.ActiveLinkColor = System.Drawing.Color.White;
            this.lnkSt.AutoSize = true;
            this.lnkSt.BackColor = System.Drawing.Color.Transparent;
            this.lnkSt.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSt.ForeColor = System.Drawing.Color.White;
            this.lnkSt.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkSt.LinkColor = System.Drawing.Color.White;
            this.lnkSt.Location = new System.Drawing.Point(60, 153);
            this.lnkSt.Name = "lnkSt";
            this.lnkSt.Size = new System.Drawing.Size(35, 17);
            this.lnkSt.TabIndex = 5;
            this.lnkSt.TabStop = true;
            this.lnkSt.Text = "Start";
            this.lnkSt.VisitedLinkColor = System.Drawing.Color.White;
            this.lnkSt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSt_LinkClicked);
            // 
            // dgvTimeSheet
            // 
            this.dgvTimeSheet.AllowUserToAddRows = false;
            this.dgvTimeSheet.AllowUserToDeleteRows = false;
            this.dgvTimeSheet.AllowUserToResizeRows = false;
            this.dgvTimeSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTimeSheet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTimeSheet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTimeSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTimeSheet.ContextMenuStrip = this.dgvContextMenu;
            this.dgvTimeSheet.Location = new System.Drawing.Point(181, 12);
            this.dgvTimeSheet.Name = "dgvTimeSheet";
            this.dgvTimeSheet.ReadOnly = true;
            this.dgvTimeSheet.RowHeadersVisible = false;
            this.dgvTimeSheet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvTimeSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTimeSheet.Size = new System.Drawing.Size(420, 225);
            this.dgvTimeSheet.TabIndex = 6;
            // 
            // dgvContextMenu
            // 
            this.dgvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateReportToolStripMenuItem});
            this.dgvContextMenu.Name = "dgvContextMenu";
            this.dgvContextMenu.Size = new System.Drawing.Size(156, 26);
            // 
            // generateReportToolStripMenuItem
            // 
            this.generateReportToolStripMenuItem.Name = "generateReportToolStripMenuItem";
            this.generateReportToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.generateReportToolStripMenuItem.Text = "Generate Report";
            // 
            // formContextMenu
            // 
            this.formContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.formContextMenu.Name = "formContextMenu";
            this.formContextMenu.Size = new System.Drawing.Size(96, 48);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // trayContextMenu
            // 
            this.trayContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.trayContextMenu.Name = "trayContextMenu";
            this.trayContextMenu.Size = new System.Drawing.Size(101, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayContextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // MainFormUX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(613, 249);
            this.ContextMenuStrip = this.formContextMenu;
            this.Controls.Add(this.dgvTimeSheet);
            this.Controls.Add(this.lnkSt);
            this.Controls.Add(this.lnkTimesheet);
            this.Controls.Add(this.lnkExit);
            this.Controls.Add(this.txtDo);
            this.Controls.Add(this.lblTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainFormUX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASM Timex";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormUX_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimeSheet)).EndInit();
            this.dgvContextMenu.ResumeLayout(false);
            this.formContextMenu.ResumeLayout(false);
            this.trayContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtDo;
        private System.Windows.Forms.Timer timex;
        private System.Windows.Forms.LinkLabel lnkExit;
        private System.Windows.Forms.LinkLabel lnkTimesheet;
        private System.Windows.Forms.LinkLabel lnkSt;
        private System.Windows.Forms.DataGridView dgvTimeSheet;
        private System.Windows.Forms.ContextMenuStrip dgvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem generateReportToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip formContextMenu;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip trayContextMenu;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.NotifyIcon trayIcon;
    }
}