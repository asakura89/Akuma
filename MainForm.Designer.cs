namespace asmTimex
{
    partial class MainForm
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
            this.btnSt = new System.Windows.Forms.Button();
            this.timex = new System.Windows.Forms.Timer(this.components);
            this.txtDo = new System.Windows.Forms.TextBox();
            this.lnkExp = new System.Windows.Forms.LinkLabel();
            this.lstBoxDo = new System.Windows.Forms.ListBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSt
            // 
            this.btnSt.Location = new System.Drawing.Point(138, 14);
            this.btnSt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSt.Name = "btnSt";
            this.btnSt.Size = new System.Drawing.Size(87, 32);
            this.btnSt.TabIndex = 0;
            this.btnSt.Text = "Start";
            this.btnSt.UseVisualStyleBackColor = true;
            this.btnSt.Click += new System.EventHandler(this.btnSt_Click);
            // 
            // timex
            // 
            this.timex.Interval = 1000;
            this.timex.Tick += new System.EventHandler(this.timex_Tick);
            // 
            // txtDo
            // 
            this.txtDo.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDo.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.txtDo.Location = new System.Drawing.Point(14, 54);
            this.txtDo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDo.Name = "txtDo";
            this.txtDo.Size = new System.Drawing.Size(210, 23);
            this.txtDo.TabIndex = 2;
            this.txtDo.Text = "What you are doing ?";
            this.txtDo.Leave += new System.EventHandler(this.txtDo_Leave);
            this.txtDo.Enter += new System.EventHandler(this.txtDo_Enter);
            // 
            // lnkExp
            // 
            this.lnkExp.AutoSize = true;
            this.lnkExp.BackColor = System.Drawing.Color.Transparent;
            this.lnkExp.LinkColor = System.Drawing.Color.Blue;
            this.lnkExp.Location = new System.Drawing.Point(174, 90);
            this.lnkExp.Name = "lnkExp";
            this.lnkExp.Size = new System.Drawing.Size(50, 18);
            this.lnkExp.TabIndex = 3;
            this.lnkExp.TabStop = true;
            this.lnkExp.Text = "expand";
            this.lnkExp.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkExp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExp_LinkClicked);
            // 
            // lstBoxDo
            // 
            this.lstBoxDo.FormattingEnabled = true;
            this.lstBoxDo.ItemHeight = 18;
            this.lstBoxDo.Location = new System.Drawing.Point(14, 135);
            this.lstBoxDo.Name = "lstBoxDo";
            this.lstBoxDo.Size = new System.Drawing.Size(210, 166);
            this.lstBoxDo.TabIndex = 4;
            this.lstBoxDo.DoubleClick += new System.EventHandler(this.lstBoxDo_DoubleClick);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Location = new System.Drawing.Point(12, 21);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 18);
            this.lblTime.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 310);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lstBoxDo);
            this.Controls.Add(this.lnkExp);
            this.Controls.Add(this.txtDo);
            this.Controls.Add(this.btnSt);
            this.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ASM Timex";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSt;
        private System.Windows.Forms.Timer timex;
        private System.Windows.Forms.TextBox txtDo;
        private System.Windows.Forms.LinkLabel lnkExp;
        private System.Windows.Forms.ListBox lstBoxDo;
        private System.Windows.Forms.Label lblTime;
    }
}

