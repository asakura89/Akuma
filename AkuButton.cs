using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Akuma
{
    public class AkuButton : Button
    {
        private readonly Color BeginGradientColor;
        private readonly Color EndGradientColor;
        private readonly Color BorderColor;

        public AkuButton() : this(Color.Transparent) { }

        public AkuButton(Color beginColor) : this(beginColor, Color.Transparent) { }

        public AkuButton(Color beginColor, Color endColor) : this(beginColor, endColor, Color.Black) { }

        public AkuButton(Color beginColor, Color endColor, Color borderColor)
        {
            if (beginColor == Color.Transparent && endColor == Color.Transparent)
            {
                BeginGradientColor = AkuColor.DefaultBeginColor;
                EndGradientColor = AkuColor.DefaultEndColor;
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
            ClientSize = new Size(90, 40);
            Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var formRect = new Rectangle(0, 0, Width, Height);
            Brush gradientBrush = EndGradientColor == Color.Transparent ?
                (Brush)new SolidBrush(BeginGradientColor) :
                (Brush)new LinearGradientBrush(formRect, BeginGradientColor, EndGradientColor, LinearGradientMode.Vertical);
            g.Clear(DefaultBackColor);
            g.FillRectangle(gradientBrush, formRect);

            if (BorderColor != Color.Transparent)
                g.DrawRectangle(new Pen(BorderColor), new Rectangle(0, 0, Width - 1, Height - 1));

            SizeF textSize = g.MeasureString(Text, Font);
            Single x = (Width/2) - (textSize.Width/2);
            Single y = (Height/2) - (textSize.Height/2);
            g.DrawString(Text, Font, new SolidBrush(Color.White), x, y);
        }
    }
}