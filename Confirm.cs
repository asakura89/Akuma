using System;
using System.Drawing;
using System.Windows.Forms;

namespace Akuma
{
    public class Confirm : AkuForm
    {
        private const Int32 MinWidth = 240;
        private const Int32 MinHeight = 140;
        private const Int32 MaxButtonHeight = 40;
        private AkuButton btnOk;
        private AkuButton btnCancel;
        private Label lblMessage;
        private readonly String confirmMessage;
        private DialogResult dialogResult;

        public Confirm(String message) : base(ColorTranslator.FromHtml("#4c4f53"), ColorTranslator.FromHtml("#161a1f"), Color.Transparent)
        {
            confirmMessage = message;
            Text = "Confirm";
            MinimumSize = Size = new Size(MinWidth, MinHeight);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            /*Graphics g = CreateGraphics();
            SizeF textSize = g.MeasureString(confirmMessage, Font);*/

            lblMessage = new Label();
            lblMessage.Font = Font;
            lblMessage.ForeColor = Color.White;
            lblMessage.BackColor = Color.Transparent;
            lblMessage.AutoSize = true;
            lblMessage.Text = confirmMessage;
            Int32 x = (Width/2) - (lblMessage.Width/2);
            lblMessage.Location = new Point(x, MaxButtonHeight);

            btnOk = new AkuButton(AkuColor.DefaultBeginColor);
            btnOk.Location = new Point(0, Height - MaxButtonHeight);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size((Width / 2) + 1, MaxButtonHeight);
            btnOk.Text = "Ok";
            btnOk.Click += (sender, args) => { dialogResult = DialogResult.OK; Close(); };

            btnCancel = new AkuButton(AkuColor.DefaultBeginColor);
            btnCancel.Location = new Point(btnOk.Width - 1, Height - MaxButtonHeight);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size((Width / 2), MaxButtonHeight);
            btnCancel.Text = "Cancel";
            btnCancel.Click += (sender, args) => { dialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lblMessage);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }

        public new DialogResult ShowDialog(IWin32Window parent)
        {
            base.ShowDialog(parent);
            return dialogResult;
        }
    }
}