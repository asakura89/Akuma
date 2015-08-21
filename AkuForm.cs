using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Akuma
{
    public class AkuForm : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, Int32 Msg, Int32 wParam, Int32 lParam);

        private const Int32 WM_NCLBUTTONDOWN = 161;
        private const Int32 HTCAPTION = 2;

        private readonly Color BeginGradientColor;
        private readonly Color EndGradientColor;
        private readonly Color BorderColor;

        public AkuForm() : this(Color.Transparent) { }

        public AkuForm(Color beginColor) : this(beginColor, Color.Transparent) { }

        public AkuForm(Color beginColor, Color endColor) : this(beginColor, endColor, Color.Black) { }

        public AkuForm(Color beginColor, Color endColor, Color borderColor)
        {
            if (beginColor == Color.Transparent && endColor == Color.Transparent)
            {
                BeginGradientColor = ColorTranslator.FromHtml("#00acae");
                EndGradientColor = ColorTranslator.FromHtml("#1e8c99");
            }
            else
            {
                BeginGradientColor = beginColor;
                EndGradientColor = endColor;
            }

            BorderColor = borderColor;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(180, 262);
            Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            StartPosition = FormStartPosition.CenterScreen;

            MouseDown += SimpleFormOnMouseDown;
        }

        private void SimpleFormOnMouseDown(Object sender, EventArgs args)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var formRect = new Rectangle(0, 0, Width, Height);
            Brush gradientBrush = EndGradientColor == Color.Transparent ?
                (Brush) new SolidBrush(BeginGradientColor) : 
                (Brush) new LinearGradientBrush(formRect, BeginGradientColor, EndGradientColor, LinearGradientMode.ForwardDiagonal);
            g.Clear(DefaultBackColor);
            g.FillRectangle(gradientBrush, formRect);

            if (BorderColor != Color.Transparent)
                g.DrawRectangle(new Pen(BorderColor), new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
