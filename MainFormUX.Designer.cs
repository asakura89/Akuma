namespace asmTimex
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
            this.lblTime = new System.Windows.Forms.Label();
            this.txtDo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
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
            this.txtDo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDo.Location = new System.Drawing.Point(12, 116);
            this.txtDo.Name = "txtDo";
            this.txtDo.Size = new System.Drawing.Size(145, 25);
            this.txtDo.TabIndex = 1;
            // 
            // MainFormUX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(169, 249);
            this.Controls.Add(this.txtDo);
            this.Controls.Add(this.lblTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainFormUX";
            this.Text = "MainFormUX";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormUX_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtDo;
    }
}