using Csv;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;


namespace EntranceManagement
{
    /// <summary>
    /// 履歴出力フォーム
    /// </summary>
    public partial class HistoriesExportForm : Form
    {
        public HistoriesExportForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// エクセル帳票用のファイル名
        /// </summary>
        const string EXCEL_REPORT_FILE_NAME = "入退室記録表.xlsx";

        /*
         * Excelの行列は0起算
         */

        /// <summary>
        /// エクセル帳票の開始行
        /// </summary>
        const int EXCEL_REPORT_START_ROW = 4;
        /// <summary>
        /// エクセル帳票の終了行
        /// </summary>
        const int EXCEL_REPORT_END_ROW = 25;
        /// <summary>
        /// エクセル帳票の「名前」の列
        /// </summary>
        const int EXCEL_REPORT_NAME_COL = 1;
        /// <summary>
        /// エクセル帳票の「入室日時」の列
        /// </summary>
        const int EXCEL_REPORT_ENTER_TIME_COL = 2;
        /// <summary>
        /// エクセル帳票の「退室日時」の列
        /// </summary>
        const int EXCEL_REPORT_LEAVE_TIME_COL = 3;
        /// <summary>
        /// エクセル帳票の「メモ」の列
        /// </summary>
        const int EXCEL_REPORT_MEMO_COL = 4;

        #region "プロパティ"

        /// <summary>
        /// 設定ファイル情報のプロパティ
        /// </summary>
        public ConfigWrapper? configuration { get; set; }

        #endregion


        #region "イベント"

        /// <summary>
        /// CSVエクスポート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = exportDefaultFileName(dtpWorkStart.Value, dtpWorkEnd.Value) + ".csv";
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.Title = "保存先を指定してください。";
            sfd.FilterIndex = 2;
            sfd.Filter = "CSVファイル(*.csv)|*.csv";


            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            exportCSV(sfd.FileName, dtpWorkStart.Value, dtpWorkEnd.Value);

            MessageBox.Show("出力しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Excelエクスポート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = exportDefaultFileName(dtpWorkStart.Value, dtpWorkEnd.Value) + ".xlsx";
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.Title = "保存先を指定してください。";
            sfd.FilterIndex = 2;
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx";

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            exportExcel(sfd.FileName, dtpWorkStart.Value, dtpWorkEnd.Value);

            MessageBox.Show("出力しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 帳票で出力ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutputReport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = exportDefaultFileName(dtpWorkStart.Value, dtpWorkEnd.Value) + ".xlsx";
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.Title = "保存先を指定してください。";
            sfd.FilterIndex = 2;
            sfd.Filter = "Excelファイル(*.xlsx)|*.xlsx";

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            exportExcelReport(sfd.FileName, dtpWorkStart.Value, dtpWorkEnd.Value);

            MessageBox.Show("出力しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttendanceExportForm_Load(object sender, EventArgs e)
        {
            initForm();
        }

        #endregion

        #region "関数"

        /// <summary>
        /// フォームを初期化する
        /// </summary>
        private void initForm()
        {
            // 前月の1日を取得
            DateTime previousMonthFirstDay = DateTime.Now.AddMonths(-1);
            previousMonthFirstDay = new DateTime(previousMonthFirstDay.Year, previousMonthFirstDay.Month, 1);

            // 前月の1日の0時0分0秒に設定
            previousMonthFirstDay = previousMonthFirstDay.Date;

            // 前月の末日を求める
            DateTime previousMonthLastDay = previousMonthFirstDay.AddMonths(1).AddDays(-1);
            // 前月の末日の0時0分0秒に設定
            previousMonthLastDay = previousMonthLastDay.Date;

            // 開始は前月の1日にしておく
            dtpWorkStart.Value = previousMonthFirstDay;
            // 終了は前月末にしておく
            dtpWorkEnd.Value = previousMonthLastDay;
        }

        /// <summary>
        /// 履歴情報を取得する
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private DataTable? getEntranceHistories(DateTime startDate, DateTime endDate)
        {
            if (configuration == null)
            {
                return null;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "enter_date", Utils.startTime(startDate) },
                { "leave_date", Utils.endTime(endDate) }
            };
            var dt = db.ExecuteQuery(@"
                SELECT
                     entrance_histories.employee_id AS employee_id
                    ,employees.name AS name
                    ,entrance_histories.enter_date
                    ,entrance_histories.leave_date
                    ,entrance_histories.memo
                FROM
                    entrance_histories
                INNER JOIN
                    employees
                ON
                    employees.id = entrance_histories.employee_id
                WHERE
                    $enter_date <= enter_date
                AND
                    leave_date <= $leave_date
                ORDER BY
                    enter_date", param);
            return dt;
        }

        /// <summary>
        /// CSVエクスポートを行う
        /// </summary>
        /// <param name="enter_date"></param>
        /// <param name="leave_date"></param>
        private void exportCSV(string savepath, DateTime enter_date, DateTime leave_date)
        {
            var dt = getEntranceHistories(enter_date, leave_date);

            var columnNames = new[] { "従業員ID", "名前", "入室日時", "退室日時", "メモ" };
            var rowsList = new List<string[]>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var row = new string[]
                    {
                        dr["employee_id"].ToString() ?? "",
                        dr["name"].ToString() ?? "",
                        dr["enter_date"].ToString() ?? "",
                        dr["leave_date"].ToString() ?? "",
                        dr["memo"].ToString() ?? "",
                    };
                    rowsList.Add(row);
                }
            }
            var rows = rowsList;
            var csv = CsvWriter.WriteToText(columnNames, rows, ',');
            File.WriteAllText(savepath, csv);
        }

        /// <summary>
        /// Excelエクスポートを行う
        /// </summary>
        /// <param name="enter_date"></param>
        /// <param name="leave_date"></param>
        private void exportExcel(string savepath, DateTime enter_date, DateTime leave_date)
        {
            var dt = getEntranceHistories(enter_date, leave_date);

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // ヘッダ行
            setExcelRowHeader(sheet);

            if (dt != null)
            {
                var idx = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    setExcelRowBody(workbook, sheet, dr, idx);
                    idx++;
                }
            }

            using (FileStream fs = File.Create(savepath))
            {
                workbook.Write(fs);
            }

        }

        /// <summary>
        /// Excelエクスポートの行ヘッダを設定する
        /// </summary>
        /// <param name="sheet"></param>
        private void setExcelRowHeader(ISheet sheet)
        {
            IRow row = sheet.CreateRow(0);
            ICell idCell = row.CreateCell(0);
            idCell.SetCellType(CellType.String);
            idCell.SetCellValue("従業員ID");

            ICell nameCell = row.CreateCell(1);
            nameCell.SetCellType(CellType.String);
            nameCell.SetCellValue("名前");

            ICell enterDateCell = row.CreateCell(2);
            enterDateCell.SetCellType(CellType.String);
            enterDateCell.SetCellValue("入室日時");

            ICell leaveDateCell = row.CreateCell(3);
            leaveDateCell.SetCellType(CellType.String);
            leaveDateCell.SetCellValue("退室日時");

            ICell memoCell = row.CreateCell(4);
            memoCell.SetCellType(CellType.String);
            memoCell.SetCellValue("メモ");
        }

        /// <summary>
        /// Excelエクスポートのデータ行を設定する
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        private void setExcelRowBody(IWorkbook book, ISheet sheet, DataRow dr, int idx)
        {
            IRow row = sheet.CreateRow(idx);

            var enterDate = dr[2].ToString() ?? "";
            var leaveDate = dr[3].ToString() ?? "";

            // 日付書式設定
            var format = book.CreateDataFormat();
            var style = book.CreateCellStyle();
            style.DataFormat = format.GetFormat("yyyy年MM月dd日HH時mm分ss秒");

            // 従業員ID
            ICell idCell = row.CreateCell(0);
            idCell.SetCellType(CellType.String);
            idCell.SetCellValue(dr[0].ToString());

            // 名前
            ICell nameCell = row.CreateCell(1);
            nameCell.SetCellType(CellType.String);
            nameCell.SetCellValue(dr[1].ToString());

            // 入室日時
            ICell workStartDateCell = row.CreateCell(2);
            workStartDateCell.CellStyle = style;
            workStartDateCell.SetCellValue(DateTime.Parse(enterDate));

            // 退室日時
            ICell workEndDateCell = row.CreateCell(3);
            workEndDateCell.CellStyle = style;
            workEndDateCell.SetCellValue(DateTime.Parse(leaveDate));

            // メモ
            ICell memoCell = row.CreateCell(4);
            memoCell.SetCellType(CellType.String);
            memoCell.SetCellValue(dr[4].ToString());
        }

        /// <summary>
        /// エクスポート時のデフォルトファイル名を返す
        /// </summary>
        /// <param name="enter_date"></param>
        /// <param name="leave_date"></param>
        /// <returns></returns>
        private string exportDefaultFileName(DateTime enter_date, DateTime leave_date)
        {
            var ss = enter_date.ToString("M");
            var es = leave_date.ToString("M");

            return "入退室記録" + ss + "～" + es;
        }


        /// <summary>
        /// Excel帳票のエクスポートを行う
        /// </summary>
        /// <param name="enter_date"></param>
        /// <param name="leave_date"></param>
        private void exportExcelReport(string savepath, DateTime enter_date, DateTime leave_date)
        {
            if (configuration == null)
            {
                throw new Exception("configuration is not set.");
            }

            var dt = getEntranceHistories(enter_date, leave_date);

            var workbook = WorkbookFactory.Create(Path.Combine(configuration.getExcelTemplateDirPath(), EXCEL_REPORT_FILE_NAME));

            if (dt != null)
            {
                setExcelReport(dt, workbook);
            }

            using (var fs = new FileStream(savepath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// Excel帳票へデータを設定する
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="workbook"></param>
        private void setExcelReport(DataTable dt, IWorkbook workbook)
        {
            var templateSheet = workbook.GetSheet("Sheet1");
            var nSheet = 1;
            var sheet = templateSheet.CopySheet(nSheet.ToString());

            var offset = 0;
            foreach (DataRow row in dt.Rows)
            {
                setExcelReportRow(row, sheet, offset);
                offset++;
                if (EXCEL_REPORT_END_ROW - EXCEL_REPORT_START_ROW < offset)
                {
                    offset = 0;
                    nSheet++;
                    sheet = templateSheet.CopySheet(nSheet.ToString());
                }
            }
            // テンプレートシートを削除
            workbook.RemoveSheetAt(0);
            // 先頭のシートをアクティブにする
            workbook.SetActiveSheet(0);
        }

        /// <summary>
        /// Excel帳票1行分にデータを設定する
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sheet"></param>
        private void setExcelReportRow(DataRow dr, ISheet sheet, int offset)
        {
            var name = dr["name"].ToString() ?? "";
            var enter_date = dr["enter_date"].ToString() ?? "";
            var leave_date = dr["leave_date"].ToString() ?? "";
            var memo = dr["memo"].ToString() ?? "";

            NPoiUtil.WriteCell(sheet, EXCEL_REPORT_NAME_COL, EXCEL_REPORT_START_ROW + offset, name);
            NPoiUtil.WriteCell(sheet, EXCEL_REPORT_ENTER_TIME_COL, EXCEL_REPORT_START_ROW + offset, enter_date);
            NPoiUtil.WriteCell(sheet, EXCEL_REPORT_LEAVE_TIME_COL, EXCEL_REPORT_START_ROW + offset, leave_date);
            NPoiUtil.WriteCell(sheet, EXCEL_REPORT_MEMO_COL, EXCEL_REPORT_START_ROW + offset, memo);
        }


        #endregion

    }
}
