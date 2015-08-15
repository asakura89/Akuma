﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Finisar.SQLite;

namespace Akuma
{
    public partial class MainFormUX : Form
    {
        #region movable form without border

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
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

            if (m.Msg == WM_NCHITTEST && m.Result.ToInt32() == HT_CLIENT && IsOnGlass(m.LParam.ToInt32()))
            {
                m.Result = new IntPtr(HT_CAPTION);
            }
        }

        private bool IsOnGlass(int lParam)
        {
            if (!isGlassEnabled())
                return false;

            int x = (lParam << 16) >> 16;
            int y = lParam >> 16;

            Point p = PointToClient(new Point(x, y));

            if (_botRect.Contains(p))
                return true;

            return false;
        } 
        #endregion

        #endregion

        private Stopwatch _st;
        private List<Task> _lstDo = new List<Task>();
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

            Size = new Size(169, 249);

            _st = new Stopwatch();
            _lstDo = new List<Task>();
            _dblocation = AppDomain.CurrentDomain.BaseDirectory + DB_NAME;
            _constring = "Data Source=" + _dblocation + ";Version=3;Compress=false;";

            _marg.Top = 0;
            _marg.Left = 0;
            _marg.Right = 0;
            _marg.Bottom = -1;

            //DwmExtendFrameIntoClientArea(Handle, ref _marg);
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
            lnkSt.Text = "Start";
            lblTime.Text = "";
            txtDo.Text = "";
            FocusChanged();
            _starttime = new DateTime();
            _endtime = new DateTime();
            timex.Stop();
            timex.Enabled = false;
        }

        private void FocusChanged()
        {
            if (txtDo.Text.Equals("What you are doing ?"))
            {
                txtDo.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                txtDo.ForeColor = SystemColors.WindowText;
                txtDo.Text = "";
            }
            else if (txtDo.Text.Equals(String.Empty))
            {
                txtDo.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Italic, GraphicsUnit.Point, ((byte)(0)));
                txtDo.ForeColor = Color.DarkGray;
                txtDo.Text = "What you are doing ?";
            }
        }

        private Task convertRaw2Repo(String strJobDate, String strJobDeskName, String strTotalTime, String strTimespan, DateTime dtStart, DateTime dtStop)
        {
            Task retJobRepo = new Task();

            retJobRepo.JobDate = strJobDate;
            retJobRepo.JobDeskName = strJobDeskName;
            retJobRepo.TotalTime = strTotalTime;
            retJobRepo.Timespan = strTimespan;
            retJobRepo.Start = dtStart;
            retJobRepo.Stop = dtStop;

            return retJobRepo;
        }

        private void setData(Task lastDo)
        {
            List<Task> lstRepo = new List<Task>();

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

        private List<Task> getData()
        {
            List<Task> retLstJob = new List<Task>();

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

        private List<Task> getData(String strJobDate)
        {
            List<Task> retLstJob = new List<Task>();

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
            try
            {
                //if (isGlassSupported == false)
                //{
                //    Rectangle BaseRectangle = new Rectangle(0, 0, Width - 1, Height - 1);

                //    Brush Gradient_Brush =
                //        new LinearGradientBrush(
                //        BaseRectangle,
                //        Color.FromArgb(76, 79, 83), Color.FromArgb(22, 26, 31),
                //        LinearGradientMode.Vertical);

                //    e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);
                //}
                //else
                //{
                //    //FormBorderStyle = FormBorderStyle.Sizable; 
                //    _botRect = ClientRectangle; //new Rectangle(0, 0, ClientSize.Width, marg.Bottom);
                //}

                Rectangle BaseRectangle = new Rectangle(0, 0, Width - 1, Height - 1);

                Brush Gradient_Brush =
                    new LinearGradientBrush(
                    BaseRectangle,
                    Color.FromArgb(76, 79, 83), Color.FromArgb(22, 26, 31),
                    LinearGradientMode.Vertical);

                e.Graphics.FillRectangle(Gradient_Brush, BaseRectangle);

                base.OnPaint(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void txtDo_Enter(object sender, EventArgs e)
        {
            try
            {
                FocusChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDo_Leave(object sender, EventArgs e)
        {
            try
            {
                FocusChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lnkExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lnkTimesheet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (Expanded)
                {
                    Expanded = false;
                    lnkTimesheet.Text = "Expand Timesheet";
                    Size = new Size(169, 249);
                }
                else
                {
                    Expanded = true;
                    lnkTimesheet.Text = "Collapse Timesheet";
                    Size = new Size(613, 249);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lnkSt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (Started)
                {
                    _st.Stop();
                    lblTime.Text = Duration;
                    _endtime = DateTime.Now;

                    Task jobdesk = new Task();
                    jobdesk.JobDate = String.Format("{0:MMM d, yyyy}", _starttime);
                    jobdesk.Start = _starttime;
                    jobdesk.Stop = _endtime;
                    jobdesk.JobDeskName = txtDo.Text;
                    jobdesk.TotalTime = Duration;
                    jobdesk.Timespan = String.Format("{0:h:mm tt}", _starttime) + " - " + String.Format("{0:h:mm tt}", _endtime);

                    _lstDo.Add(jobdesk);
                    dgvTimeSheet.DataSource = null;
                    dgvTimeSheet.DataSource = _lstDo;
                    dgvTimeSheet.Columns["Start"].Visible = false;
                    dgvTimeSheet.Columns["Stop"].Visible = false;

                    ResetTimex();


                }
                else
                {
                    _st.Start();
                    timex.Enabled = true;
                    timex.Start();
                    Started = true;
                    _starttime = DateTime.Now;
                    lblTime.Text = "";
                    lnkSt.Text = "Stop";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timex_Tick(object sender, EventArgs e)
        {
            try
            {
                lblTime.Text = Duration;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    Hide();
                    WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
