using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace asmTimex
{
    public partial class MainFormUX : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainFormUX()
        {
            InitializeComponent();

            this.Size = new Size(169, 249);
        }

        private void MainFormUX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle BaseRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Brush Gradient_Brush =
                new LinearGradientBrush(
                BaseRectangle,
                Color.FromArgb(76, 79, 83), Color.FromArgb(22, 26, 31),
                LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);

            base.OnPaint(e);
        }
    }
}
