using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using Finisar.SQLite;

namespace asmTimex
{
    public partial class MainFormUX : Form
    {
        #region movable form without border

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MainFormUX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region aero glass
        
        public struct Margins
        {
            public int Left, Right, Top, Bottom;
        }

        public const int WM_NCHITTEST = 0x84;
        public const int HT_CLIENT = 1;
        public const int HTCAPTION = 2;

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        [DllImport("dwmapi.dll")]
        public static extern void DwmIsCompositionEnabled(ref bool isEnabled);

        private Margins _marg;
        private Rectangle _botRect = Rectangle.Empty;
        private bool isGlassSupported = true;

        private bool isGlassEnabled()
        {
            //if (Environment.OSVersion.Version.Major < 6)
            //{
            //    isGlassSupported = false;
            //}

            DwmIsCompositionEnabled(ref isGlassSupported);

            return isGlassSupported;
        }

        #region moving glass
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && m.Result.ToInt32() == HT_CLIENT && this.IsOnGlass(m.LParam.ToInt32()))
            {
                m.Result = new IntPtr(HT_CAPTION);
            }
        }

        private bool IsOnGlass(int lParam)
        {
            if (!this.isGlassEnabled())
                return false;

            int x = (lParam << 16) >> 16;
            int y = lParam >> 16;

            Point p = this.PointToClient(new Point(x, y));

            if (_botRect.Contains(p))
                return true;

            return false;
        } 
        #endregion

        #endregion

        private Stopwatch _st;
        private List<JobDeskRepository> _lstDo = new List<JobDeskRepository>();
        private DateTime _starttime;
        private DateTime _endtime;

        private const String DB_NAME = "log.s3db";
        private const String DB_CREATE_CMD = "create table logtbl(JobDate varchar(200),Start datetime,Stop datetime,JobDeskName varchar(1000),TotalTime varchar(200),Timespan varchar(200))";


        private String _dblocation = String.Empty;
        private String _constring = String.Empty;

        private Boolean _started = false;
        private Boolean _expanded = false;

        private Boolean Started { get { return _started; } set { _started = value; } }
        private Boolean Expanded { get { return _expanded; } set { _expanded = value; } }

        private DBConnection _dbcon = DBConnection.GetInstance;
        private SQLiteConnection _sqlcon = new SQLiteConnection();
        private SQLiteDataReader sqlread = null;

        public MainFormUX()
        {
            InitializeComponent();

            this.Size = new Size(169, 249);

            _st = new Stopwatch();
            _lstDo = new List<JobDeskRepository>();
            _dblocation = AppDomain.CurrentDomain.BaseDirectory + DB_NAME;
            _constring = "Data Source=" + _dblocation + ";Version=3;Compress=false;";

            _marg.Top = 0;
            _marg.Left = 0;
            _marg.Right = 0;
            _marg.Bottom = -1;

            //DwmExtendFrameIntoClientArea(this.Handle, ref _marg);
        }

        private String Duration
        {
            get
            {
                String strElapsed = "";
                Int64 elapsedMil = _st.ElapsedMilliseconds;
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
            _st.Reset();
            this.lnkSt.Text = "Start";
            this.lblTime.Text = "";
            this.txtDo.Text = "";
            FocusChanged();
            this._starttime = new DateTime();
            this._endtime = new DateTime();
            this.timex.Stop();
            this.timex.Enabled = false;
        }

        private void FocusChanged()
        {
            if (this.txtDo.Text.Equals("What you are doing ?"))
            {
                this.txtDo.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                this.txtDo.ForeColor = SystemColors.WindowText;
                this.txtDo.Text = "";
            }
            else if (this.txtDo.Text.Equals(String.Empty))
            {
                this.txtDo.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Italic, GraphicsUnit.Point, ((byte)(0)));
                this.txtDo.ForeColor = Color.DarkGray;
                this.txtDo.Text = "What you are doing ?";
            }
        }

        private JobDeskRepository convertRaw2Repo(String strJobDate, String strJobDeskName, String strTotalTime, String strTimespan, DateTime dtStart, DateTime dtStop)
        {
            JobDeskRepository retJobRepo = new JobDeskRepository();

            retJobRepo.JobDate = strJobDate;
            retJobRepo.JobDeskName = strJobDeskName;
            retJobRepo.TotalTime = strTotalTime;
            retJobRepo.Timespan = strTimespan;
            retJobRepo.Start = dtStart;
            retJobRepo.Stop = dtStop;

            return retJobRepo;
        }

        private void setData(JobDeskRepository lastDo)
        {
            List<JobDeskRepository> lstRepo = new List<JobDeskRepository>();

            try
            {
                if (File.Exists(_dblocation))
                {
                    String strCon = _constring + "New=false;";

                    _sqlcon = _dbcon.OpenConnection(strCon);
                    _sqlcon.Open();

                    SQLiteCommand sqlcmd = _sqlcon.CreateCommand();
                    SQLiteParameter sqlparam = new SQLiteParameter();
                    sqlcmd.CommandText = "insert into log values(?,?,?,?,?,?)";

                    sqlparam = new SQLiteParameter("@JobDate", DbType.String);
                    sqlparam.Value = lastDo.JobDate;
                    sqlcmd.Parameters.Add(sqlparam);

                    sqlparam = new SQLiteParameter("@Start", DbType.DateTime);
                    sqlparam.Value = lastDo.Start;
                    sqlcmd.Parameters.Add(sqlparam);

                    sqlparam = new SQLiteParameter("@Stop", DbType.DateTime);
                    sqlparam.Value = lastDo.Stop;
                    sqlcmd.Parameters.Add(sqlparam);

                    sqlparam = new SQLiteParameter("@JobDeskName", DbType.String);
                    sqlparam.Value = lastDo.JobDeskName;
                    sqlcmd.Parameters.Add(sqlparam);

                    sqlparam = new SQLiteParameter("@TotalTime", DbType.String);
                    sqlparam.Value = lastDo.TotalTime;
                    sqlcmd.Parameters.Add(sqlparam);

                    sqlparam = new SQLiteParameter("@Timespan", DbType.String);
                    sqlparam.Value = lastDo.Timespan;
                    sqlcmd.Parameters.Add(sqlparam);

                    Int32 rowAffected = sqlcmd.ExecuteNonQuery();

                    if (rowAffected <= 0) { throw new Exception("Data not inserted"); }
                }
                else
                {
                    //String strCon = CON_STRING + "New=true;";

                    //_sqlcon = _dbcon.OpenConnection(strCon);
                    //_sqlcon.Open();

                    //SQLiteCommand sqlcmd = _sqlcon.CreateCommand();
                    //sqlcmd.CommandText = DB_CREATE_CMD;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlread != null) { sqlread.Close(); sqlread.Dispose(); }
                if (_sqlcon != null) { _sqlcon.Close(); _sqlcon.Dispose(); }
            }
        }

        private List<JobDeskRepository> getData()
        {
            List<JobDeskRepository> retLstJob = new List<JobDeskRepository>();

            try
            {
                if (File.Exists(_dblocation))
                {
                    String strCon = _constring + "New=false;";

                    _sqlcon = _dbcon.OpenConnection(strCon);
                    _sqlcon.Open();

                    SQLiteCommand sqlcmd = _sqlcon.CreateCommand();
                    sqlcmd.CommandText = "select * from log";
                    sqlread = sqlcmd.ExecuteReader();

                    while (sqlread.Read())
                    {
                        retLstJob.Add(convertRaw2Repo(sqlread["JobDate"].ToString(), sqlread["JobDeskName"].ToString(), sqlread["TotalTime"].ToString(), sqlread["Timespan"].ToString(), Convert.ToDateTime(sqlread["Start"]), Convert.ToDateTime(sqlread["Stop"])));
                    }
                }
                else { throw new Exception("Database is't found"); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlread != null) { sqlread.Close(); sqlread.Dispose(); }
                if (_sqlcon != null) { _sqlcon.Close(); _sqlcon.Dispose(); }
            }

            return retLstJob;
        }

        private List<JobDeskRepository> getData(String strJobDate)
        {
            List<JobDeskRepository> retLstJob = new List<JobDeskRepository>();

            try
            {
                if (File.Exists(_dblocation))
                {
                    String strCon = _constring + "New=false;";

                    _sqlcon = _dbcon.OpenConnection(strCon);
                    _sqlcon.Open();

                    SQLiteCommand sqlcmd = _sqlcon.CreateCommand();
                    sqlcmd.CommandText = "select * from log where JobDate = '" + strJobDate + "'";
                    sqlread = sqlcmd.ExecuteReader();

                    while (sqlread.Read())
                    {
                        retLstJob.Add(convertRaw2Repo(sqlread["JobDate"].ToString(), sqlread["JobDeskName"].ToString(), sqlread["TotalTime"].ToString(), sqlread["Timespan"].ToString(), Convert.ToDateTime(sqlread["Start"]), Convert.ToDateTime(sqlread["Stop"])));
                    }
                }
                else { throw new Exception("Database is't found"); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlread != null) { sqlread.Close(); sqlread.Dispose(); }
                if (_sqlcon != null) { _sqlcon.Close(); _sqlcon.Dispose(); }
            }

            return retLstJob;
        }

        #region gradient panel and aero glass

        protected override void OnPaint(PaintEventArgs e)
        {
            //if (isGlassSupported == false)
            //{
            //    Rectangle BaseRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            //    Brush Gradient_Brush =
            //        new LinearGradientBrush(
            //        BaseRectangle,
            //        Color.FromArgb(76, 79, 83), Color.FromArgb(22, 26, 31),
            //        LinearGradientMode.Vertical);

            //    e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);
            //}
            //else
            //{
            //    //this.FormBorderStyle = FormBorderStyle.Sizable; 
            //    _botRect = this.ClientRectangle; //new Rectangle(0, 0, this.ClientSize.Width, marg.Bottom);
            //}

            Rectangle BaseRectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Brush Gradient_Brush =
                new LinearGradientBrush(
                BaseRectangle,
                Color.FromArgb(76, 79, 83), Color.FromArgb(22, 26, 31),
                LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);

            base.OnPaint(e);
        }

        #endregion

        private void txtDo_Enter(object sender, EventArgs e)
        {
            FocusChanged();
        }

        private void txtDo_Leave(object sender, EventArgs e)
        {
            FocusChanged();
        }

        private void lnkExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void lnkTimesheet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Expanded)
            {
                Expanded = false;
                this.lnkTimesheet.Text = "Expand Timesheet";
                this.Size = new Size(169, 249);
            }
            else
            {
                Expanded = true;
                this.lnkTimesheet.Text = "Collapse Timesheet";
                this.Size = new Size(613, 249);
            }
        }

        private void lnkSt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Started)
            {
                this._st.Stop();
                this.lblTime.Text = Duration;
                this._endtime = DateTime.Now;

                JobDeskRepository jobdesk = new JobDeskRepository();
                jobdesk.JobDate = String.Format("{0:MMM d, yyyy}", this._starttime);
                jobdesk.Start = this._starttime;
                jobdesk.Stop = this._endtime;
                jobdesk.JobDeskName = this.txtDo.Text;
                jobdesk.TotalTime = Duration;
                jobdesk.Timespan = String.Format("{0:h:mm tt}", this._starttime) + " - " + String.Format("{0:h:mm tt}", this._endtime);

                this._lstDo.Add(jobdesk);
                this.dgvTimeSheet.DataSource = null;
                this.dgvTimeSheet.DataSource = this._lstDo;
                this.dgvTimeSheet.Columns["Start"].Visible = false;
                this.dgvTimeSheet.Columns["Stop"].Visible = false;

                ResetTimex();


            }
            else
            {
                _st.Start();
                this.timex.Enabled = true;
                this.timex.Start();
                Started = true;
                this._starttime = DateTime.Now;
                this.lblTime.Text = "";
                this.lnkSt.Text = "Stop";
            }
        }

        private void timex_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = Duration;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
