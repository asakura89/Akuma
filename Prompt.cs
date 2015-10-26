using System;
using System.Drawing;
using System.Windows.Forms;

namespace Akuma
{
    public class Prompt : AkuForm
    {
        private const Int32 MinWidth = 240;
        private const Int32 MinHeight = 140;
        private const Int32 MaxButtonHeight = 40;
        private AkuButton btnOk;
        private AkuButton btnCancel;
        private Label lblMessage;
        private TextBox txtResult;
        private readonly String promptMessage;
        private String dialogResult;

        public Prompt(String message) : base(ColorTranslator.FromHtml("#4c4f53"), ColorTranslator.FromHtml("#161a1f"), Color.Transparent)
        {
            promptMessage = message;
            Text = "Prompt";
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
            lblMessage.Text = promptMessage;
            Int32 x = (Width/2) - (lblMessage.Width/2);
            lblMessage.Location = new Point(x, MaxButtonHeight);

            txtResult = new TextBox();
            txtResult.Font = Font;
            txtResult.BackColor = Color.White;
            txtResult.ForeColor = Color.DarkGray;
            txtResult.BorderStyle = BorderStyle.FixedSingle;
            txtResult.Size = new Size(Width - 10, 25);
            txtResult.Location = new Point(10, MaxButtonHeight + lblMessage.Height + 10);

            btnOk = new AkuButton(AkuColor.DefaultBeginColor);
            btnOk.Location = new Point(0, Height - MaxButtonHeight);
            btnOk.Size = new Size((Width / 2) + 1, MaxButtonHeight);
            btnOk.Text = "Ok";
            btnOk.Click += (sender, args) => { dialogResult = txtResult.Text; Close(); };

            btnCancel = new AkuButton(AkuColor.DefaultBeginColor);
            btnCancel.Location = new Point(btnOk.Width - 1, Height - MaxButtonHeight);
            btnCancel.Size = new Size((Width / 2), MaxButtonHeight);
            btnCancel.Text = "Cancel";
            btnCancel.Click += (sender, args) => { dialogResult = null; Close(); };

            Controls.Add(lblMessage);
            Controls.Add(txtResult);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
        }

        public new String ShowDialog(IWin32Window parent)
        {
            base.ShowDialog(parent);
            return dialogResult;
        }
    }
}