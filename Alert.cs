using System;
using System.Drawing;
using System.Windows.Forms;

namespace Akuma
{
    public class Alert : AkuForm
    {
        private const Int32 MinWidth = 240;
        private const Int32 MinHeight = 140;
        private const Int32 MaxButtonHeight = 40;
        private AkuButton btnOk;
        private Label lblMessage;
        private readonly String alertMessage;

        public Alert(String message) : base(ColorTranslator.FromHtml("#4c4f53"), ColorTranslator.FromHtml("#161a1f"), Color.Transparent)
        {
            alertMessage = message;
            Text = "Alert";
            MinimumSize = Size = new Size(MinWidth, MinHeight);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblMessage = new Label();
            lblMessage.Font = Font;
            lblMessage.ForeColor = Color.White;
            lblMessage.BackColor = Color.Transparent;
            lblMessage.AutoSize = true;
            lblMessage.Text = alertMessage;
            Int32 x = (Width/2) - (lblMessage.Width/2);
            Int32 y = (Height/2) - (lblMessage.Height/2);
            lblMessage.Location = new Point(x, MaxButtonHeight);

            btnOk = new AkuButton(AkuColor.DefaultBeginColor);
            btnOk.Location = new Point(0, Height - MaxButtonHeight);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(Width, MaxButtonHeight);
            btnOk.Text = "Ok";
            btnOk.Click += (sender, args) => Close();

            Controls.Add(lblMessage);
            Controls.Add(btnOk);
        } 
    }
}