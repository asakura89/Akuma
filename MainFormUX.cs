using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Akuma.Model;
using Databossy;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

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

        private const String Version = "0.1";
        private const String ExpandText = "Expand";
        private const String CollapseText = "Collapse";
        private const String StartText = "Start";
        private const String StopText = "Stop";
        private const String WhatRUDoingText = "What you are doing ?";
        private const String ExceptionTitleText = "No no no my friend!";
        private const String InformTitleText = "Ja jang!";

        private Boolean IsStarted = false;
        private Boolean IsUIExpanded = false;

        private DateTime startTime;
        private DateTime endTime;
        private TimeSpan taskDuration;

        public MainFormUX() : base(ColorTranslator.FromHtml("#4c4f53"), ColorTranslator.FromHtml("#161a1f"), Color.Transparent)
        {
            InitializeComponent();
            InitializeSqliteDbProvider();
            InitializeDatabase();
            InitializeData();

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
                        db.QueryScalar<Int64>("SELECT EXISTS (SELECT * FROM sqlite_master WHERE type = 'table' AND name = 'Akuma');"));

                    if (!isTableExist)
                        CreateTimexSchema(db);

                    trx.Complete();
                }
            }
        }

        private static void CreateTimexSchema(Database db)
        {
            String ddlQuery =
                @"CREATE TABLE [Akuma]
                (
                    [Version] VARCHAR(6)
                );

                CREATE TABLE [TaskList]
                (
                    [Id] VARCHAR(40) PRIMARY KEY NOT NULL,
                    [Title] VARCHAR(256) NOT NULL
                );

                CREATE TABLE [Task]
                (
                    [Id] VARCHAR(40) NOT NULL,
                    [ListId] VARCHAR(40) NOT NULL,
                    [Title] VARCHAR(256) NOT NULL,
                    [Start] DATETIME NOT NULL,
                    [Stop] DATETIME NOT NULL,
                    [TotalTime] VARCHAR(200),
                    [Timespan] VARCHAR(200),
                    PRIMARY KEY (Id, ListId)
                );

                INSERT INTO Akuma VALUES (@0);
                INSERT INTO TaskList VALUES ('Home', 'Home')";

            db.Execute(ddlQuery, Version);
        }

        private void InitializeData()
        {
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
            {
                IEnumerable<TaskList> taskList = db.Query<TaskList>("SELECT * FROM TaskList");
                cmbTaskList.DataSource = taskList.ToList();
                cmbTaskList.DisplayMember = "Title";
                cmbTaskList.ValueMember = "Id";
                if (cmbTaskList.Items.Count > 0)
                    cmbTaskList.SelectedIndex = 0;
            }
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
                    hourDuration = currentHour.ToString().PadLeft(2, '0') + "h";
                if (currentMin > 0)
                    minDuration = currentMin.ToString().PadLeft(2, '0') + "m";
            }
            
            return String.Format("{0} {1} {2}", hourDuration, minDuration, currentTick.ToString().PadLeft(2, '0') + "s").Trim();
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

        private void cmbTaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void lnkStart_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtTask.Text) || txtTask.Text == WhatRUDoingText)
                    throw new InvalidOperationException("You forgot to tell me what task it is.\r\n[Hint] Lookie the textbox above start button.");

                if (IsStarted)
                {
                    timer.Stop();
                    endTime = DateTime.Now;
                    IsStarted = false;

                    SaveTask();
                    RefreshGrid();

                    lblTime.Text = String.Empty;
                    txtTask.Text = String.Empty;
                    txtTask.ReadOnly = false;
                    lnkStart.Text = StartText;

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
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void SaveTask()
        {
            String taskId = Keywielder.Keywielder.New().AddGUIDString().BuildKey();
            String listId = ((TaskList) cmbTaskList.SelectedItem ?? new TaskList()).Id;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                db.Execute("INSERT INTO Task VALUES (@0, @1, @2, @3, @4, @5, @6)", taskId, listId, txtTask.Text, startTime, endTime,
                    CalculateDuration(), String.Format("{0:[h:mm:ss:tt}", startTime) + "-" + String.Format("{0:h:mm:ss:tt]}", endTime));
        }

        private void RefreshGrid()
        {
            DataTable dt = GetTaskListByCurrentSelectedListId();

            dgvTask.DataSource = dt;
            dgvTask.Columns["Id"].Visible = false;
            dgvTask.Columns["ListId"].Visible = false;
            dgvTask.Columns["Start"].Visible = false;
            dgvTask.Columns["Stop"].Visible = false;
            dgvTask.Columns["ListTitle"].Visible = false;
        }

        private DataTable GetTaskListByCurrentSelectedListId()
        {
            DataTable dt = null;
            String listId = ((TaskList)cmbTaskList.SelectedItem ?? new TaskList()).Id;
            using (var db = new Database(ConnectionString, Database.ConnectionStringType.ConnectionString, Provider))
                dt = db.QueryDataTable("SELECT t.*, tl.Title ListTitle FROM Task t JOIN TaskList tl ON t.ListId = tl.[Id] WHERE ListId = @0", listId);

            return dt;
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void newListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                new Confirm("You sure want to Confirm this?").ShowDialog(this);
                new Alert("This is Alert!").ShowDialog(this);
                new Prompt("What's your favorite color?").ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = GetTaskListByCurrentSelectedListId();
                ExportToExcelFormat(dt);

                MessageBox.Show("Done.", InformTitleText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void ExportToExcelFormat(DataTable dt)
        {
            const String FileExtension = ".xls";
            var workbook = CreateExcelSheet(dt);
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                File.WriteAllBytes(GetExportedAkumaFilename() + FileExtension, stream.GetBuffer());
            }
        }

        private HSSFWorkbook CreateExcelSheet(DataTable dt)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Akuma");
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            font.FontName = "Arial";
            font.Boldweight = (Int16)FontBoldWeight.BOLD;

            var style = workbook.CreateCellStyle();
            style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.GREY_25_PERCENT.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;
            style.Alignment = HorizontalAlignment.CENTER;

            CreateHeader(ref sheet, style, font, dt);
            PopulateData(ref sheet, dt);

            return workbook;
        }

        private void CreateHeader(ref ISheet sheet, ICellStyle style, IFont font, DataTable dt)
        {
            var header = sheet.CreateRow(0);
            var columnHeader = new String[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                columnHeader[i] = dt.Columns[i].ColumnName;

            for (int colIdx = 0; colIdx < columnHeader.Length; colIdx++)
            {
                var cell = header.CreateCell(colIdx);
                cell.SetCellValue(columnHeader[colIdx]);
                cell.CellStyle = style;
                cell.CellStyle.SetFont(font);
            }
        }

        private void PopulateData(ref ISheet sheet, DataTable dt)
        {
            for (int rowIdx = 1; rowIdx < dt.Rows.Count; rowIdx++)
            {
                var row = sheet.CreateRow(rowIdx);
                Int32 columnLength = dt.Columns.Count;
                for (int colIdx = 0; colIdx < columnLength; colIdx++)
                {
                    row.CreateCell(colIdx).SetCellValue(dt.Rows[rowIdx][colIdx].ToString());
                    sheet.AutoSizeColumn(colIdx);
                }
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = GetTaskListByCurrentSelectedListId();
                ExportToTodotxtFormat(dt);

                MessageBox.Show("Done.", InformTitleText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void ExportToTodotxtFormat(DataTable dt)
        {
            const String FileExtension = ".txt";
            var todotxtList = new StringBuilder();
            DataRowCollection drList = dt.Rows;
            foreach (DataRow dr in drList)
            {
                var todotxt = String.Format("{0} {1} +{2} {3} {4}", Convert.ToDateTime(dr["Start"]).ToString("yyyy-MM-dd"), dr["Title"],
                    dr["ListTitle"], dr["TotalTime"], dr["Timespan"]);
                todotxtList.AppendLine(todotxt);
            }

            File.AppendAllText(GetExportedAkumaFilename() + FileExtension, todotxtList.ToString(), Encoding.UTF8);
        }

        private String GetExportedAkumaFilename()
        {
            return "Exported_Akuma_" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
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
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult isExit = MessageBox.Show(this, "Sure to exit?", "Confirmation", MessageBoxButtons.YesNo);
                if (isExit == DialogResult.Yes)
                    Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ExceptionTitleText);
            }
        }
    }
}
