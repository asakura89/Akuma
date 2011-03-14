using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace asmTimex
{
    public partial class MainForm : Form
    {
        private Stopwatch st;

        private Dictionary<String, Object> lstDo;

        private Boolean _started = false;
        private Boolean _expanded = false;

        private Boolean Started { get { return _started; } set { _started = value; } }
        private Boolean Expanded { get { return _expanded; } set { _expanded = value; } }

        public MainForm()
        {
            InitializeComponent();

            st = new Stopwatch();
            lstDo = new Dictionary<String, Object>();
        }

        private String Duration
        {
            get
            {
                String strElapsed = "";
                Int64 elapsedMil = st.ElapsedMilliseconds;
                Int64 elapsedSec = elapsedMil / 1000;
                Int64 elapsedMin = elapsedSec / 60;
                Int64 elapsedHrs = elapsedMin / 60;

                if (elapsedMin > 60)
                {
                    if (Started)
                    {
                        strElapsed = String.Format("{0} hrs {1} min {2} sec", elapsedHrs, elapsedMin - (elapsedHrs * 60), elapsedSec - (elapsedMin * 60));
                    }
                    else if (!Started)
                    {
                        strElapsed = String.Format("{0} hrs {1} min {2} sec {3} mil", elapsedHrs, elapsedMin - (elapsedHrs * 60), elapsedSec - (elapsedMin * 60), elapsedMil - (elapsedSec * 1000));
                    }
                }
                else if (elapsedSec > 60)
                {
                    if (Started)
                    {
                        strElapsed = String.Format("{0} min {1} sec", elapsedMin, elapsedSec - (elapsedMin * 60));
                    }
                    else if (!Started)
                    {
                        strElapsed = String.Format("{0} min {1} sec {2} mil", elapsedMin, elapsedSec - (elapsedMin * 60), elapsedMil - (elapsedSec * 1000));
                    }
                }
                else if (elapsedSec < 60)
                {
                    if (Started)
                    {
                        strElapsed = String.Format("{0} sec", elapsedSec);
                    }
                    else if (!Started)
                    {
                        strElapsed = String.Format("{0} sec {1} mil", elapsedSec, elapsedMil - (elapsedSec * 1000));
                    }
                }

                return strElapsed;
            }
        }

        private void ResetTimex()
        {
            Started = false;
            st.Reset();
            this.btnSt.Text = "Start";
            //this.lblTime.Text = "";
            this.timex.Stop();
            this.timex.Enabled = false;
        }

        private void FocusChanged()
        {
            if (this.txtDo.Text.Equals("What you are doing ?"))
            {
                this.txtDo.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.txtDo.ForeColor = System.Drawing.SystemColors.WindowText;
                this.txtDo.Text = "";
            }
            else if (this.txtDo.Text.Equals(String.Empty))
            {
                this.txtDo.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.txtDo.ForeColor = System.Drawing.SystemColors.ScrollBar;
                this.txtDo.Text = "What you are doing ?";
            }
        }

        private void btnSt_Click(object sender, EventArgs e)
        {
            if (Started)
            {
                st.Stop();
                this.lblTime.Text = Duration;
                lstDo.Add(this.txtDo.Text, Duration);
                lstBoxDo.Items.Add(this.txtDo.Text);
                ResetTimex();
            }
            else
            {
                st.Start();
                this.timex.Enabled = true;
                this.timex.Start();
                Started = true;
                this.lblTime.Text = "";
                this.btnSt.Text = "Stop";
            }
        }

        private void timex_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = Duration;
        }

        private void txtDo_Enter(object sender, EventArgs e)
        {
            FocusChanged();
        }

        private void txtDo_Leave(object sender, EventArgs e)
        {
            FocusChanged();
        }

        private void lstBoxDo_DoubleClick(object sender, EventArgs e)
        {
            String strKey = lstBoxDo.SelectedItem.ToString();
            String strValue = lstDo[strKey].ToString();

            MessageBox.Show(strValue, strKey);
        }

        private void lnkExp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Expanded)
            {
                Expanded = false;
                this.lnkExp.Text = "expand";
            }
            else
            {
                Expanded = true;
                this.lnkExp.Text = "collapse";
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ImageAttributes attr = new ImageAttributes();
            Bitmap bmp = (Bitmap)Bitmap.FromFile(Application.StartupPath + "/bg.png");
            GraphicsUnit pu = GraphicsUnit.Pixel;

            attr.SetColorKey(bmp.GetPixel(0, 0), bmp.GetPixel(0, 0));
            e.Graphics.FillRectangle(Brushes.Transparent, this.DisplayRectangle);
            e.Graphics.DrawImage((Image)bmp,
                Rectangle.Truncate(bmp.GetBounds(ref pu)),
                                    0F, 0F, (Single)bmp.Width,
                                    (Single)bmp.Height,
                                    GraphicsUnit.Pixel, attr);

            base.OnPaint(e);
        }
    }
}
