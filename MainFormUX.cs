using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;
using Databossy;

namespace Akuma
{
    public partial class MainFormUX : AkuForm
    {
        private const String Provider = "System.Data.SQLite";
        private readonly String ConnectionString = "DataSource=" + AppDomain.CurrentDomain.BaseDirectory + "history.akm;Version=3;Compress=True;UTF8Encoding=True;Page Size=1024;FailIfMissing=False;Read Only=False;Pooling=True;Max Pool Size=100;";

        private const Int32 DefaultWidth = 169;
        private const Int32 DefaultHeight = 249;
        private const Int32 ExpandedWidth = 613;
        private const Int32 ExpandedHeight = 249;

        private const String ExpandText = "Expand";
        private const String CollapseText = "Collapse";
        private const String StartText = "Start";
        private const String StopText = "Stop";
        private const String WhatRUDoingText = "What you are doing ?";

        private Boolean IsStarted = false;
        private Boolean IsUIExpanded = false;

        private DateTime startTime;
        private DateTime endTime;
        private TimeSpan taskDuration;

        public MainFormUX()
        {
            InitializeComponent();
            InitializeSqliteDbProvider();
            InitializeDatabase();

            Size = new Size(DefaultWidth, DefaultHeight);
        }

        private void InitializeSqliteDbProvider()
        {
            try
            {
                var configDs = ConfigurationManager.GetSection("system.data") as DataSet;
                if (configDs != null)
                    configDs.Tables[0]
                        .Rows.Add("SQLite Data Provider", ".Net Framework Data Provider for SQLite",
                            Provider, "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
            }
            catch { }
        }

        private void InitializeDatabase()
        {
            using (var trx = new TransactionScope())
            {
                using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                {
                    // NOTE: EXISTS in sqlite return object {long} type and value is case-sensitive
                    Boolean isTableExist = Convert.ToBoolean(
                        db.QueryScalar<Int64>("SELECT EXISTS (SELECT * FROM sqlite_master WHERE type = 'table' AND name = 'Task');"));

                    if (!isTableExist)
                        CreateTimexSchema(db);

                    trx.Complete();
                }
            }
        }

        private static void CreateTimexSchema(Database db)
        {
            String ddlQuery = @"CREATE TABLE Task
                            (
                                JobDate VARCHAR(200),
                                Start DATETIME,
                                Stop DATETIME,
                                JobDeskName VARCHAR(1000),
                                TotalTime VARCHAR(200),
                                Timespan VARCHAR(200)
                            );";

            db.Execute(ddlQuery);
        }

        private String CalculateDuration()
        {
            String hourDuration = String.Empty;
            String minDuration = String.Empty;

            Int32 currentTick = 0;
            if (startTime != new DateTime(1, 1, 1))
            {
                currentTick = taskDuration.Seconds;
                Int32 currentMin = taskDuration.Minutes;
                Int32 currentHour = taskDuration.Hours;
                if (currentHour > 0)
                    hourDuration = currentHour.ToString().PadLeft(2, '0') + " h";
                if (currentMin > 0)
                    minDuration = currentMin.ToString().PadLeft(2, '0') + " m";
            }
            
            return String.Format("{0} {1} {2}", hourDuration, minDuration, currentTick.ToString().PadLeft(2, '0') + " s").Trim();
        }

        private void FocusChanged()
        {
            if (txtTask.Text.Equals(WhatRUDoingText))
            {
                txtTask.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                txtTask.ForeColor = SystemColors.WindowText;
                txtTask.Text = String.Empty;
            }
            else if (txtTask.Text.Equals(String.Empty))
            {
                txtTask.Font = new Font("Trebuchet MS", 9.75F, FontStyle.Italic, GraphicsUnit.Point, ((byte)(0)));
                txtTask.ForeColor = Color.DarkGray;
                txtTask.Text = WhatRUDoingText;
            }
        }

        private void txtTask_Enter(object sender, EventArgs e)
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

        private void txtTask_Leave(object sender, EventArgs e)
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

        private void lnkExpand_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (IsUIExpanded)
                {
                    IsUIExpanded = false;
                    lnkExpand.Text = ExpandText;
                    Size = new Size(DefaultWidth, DefaultHeight);
                }
                else
                {
                    IsUIExpanded = true;
                    lnkExpand.Text = CollapseText;
                    Size = new Size(ExpandedWidth, ExpandedHeight);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lnkStart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (IsStarted)
                {
                    timer.Stop();
                    endTime = DateTime.Now;
                    IsStarted = false;

                    lblTime.Text = String.Empty;
                    txtTask.Text = String.Empty;
                    txtTask.ReadOnly = false;
                    lnkStart.Text = StartText;
                    
                    SaveTask();
                    RefreshGrid();

                    startTime = new DateTime(1, 1, 1);
                }
                else
                {
                    timer.Start();
                    startTime = DateTime.Now;
                    endTime = new DateTime(1, 1, 1);
                    IsStarted = true;

                    lblTime.Text = String.Empty;
                    txtTask.ReadOnly = true;
                    lnkStart.Text = StopText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveTask()
        {
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                db.Execute("INSERT INTO Task VALUES (@0, @1, @2, @3, @4, @5)", startTime.ToString("dd MMMM yyyy"), startTime, endTime,
                    txtTask.Text, CalculateDuration(), String.Format("{0:h:mm tt}", startTime) + " - " + String.Format("{0:h:mm tt}", endTime));
        }

        private void RefreshGrid()
        {
            DataTable dt = null;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                dt = db.QueryDataTable("SELECT * FROM Task");

            dgvTask.DataSource = dt;
            dgvTask.Columns["Start"].Visible = false;
            dgvTask.Columns["Stop"].Visible = false;
        }

        private void timex_Tick(object sender, EventArgs e)
        {
            try
            {
                taskDuration = DateTime.Now - startTime;
                lblTime.Text = CalculateDuration();
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
