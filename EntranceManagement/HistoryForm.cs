using System.Data;

namespace EntranceManagement
{
    /// <summary>
    /// 勤怠履歴
    /// </summary>
    public partial class HistoryForm : Form
    {
        #region "プロパティ"

        /// <summary>
        /// 設定ファイル情報のプロパティ
        /// </summary>
        public ConfigWrapper? configuration { get; set; }

        #endregion


        public HistoryForm()
        {
            InitializeComponent();
        }

        #region "イベント"

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryForm_Load(object sender, EventArgs e)
        {
            initForm();
        }

        /// <summary>
        /// 検索ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var employee_id = cbemployee.SelectedValue.ToString() ?? "";
            searchEntranceHistories(employee_id, dtpWorkStart.Value, dtpWorkEnd.Value);
        }

        /// <summary>
        /// 削除ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("選択中の行を削除しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            var id = dgvHistory.SelectedRows[0].Cells["colId"].Value;
            deleteAttendance((long)id);

            var employee_id = cbemployee.SelectedValue.ToString() ?? "";
            searchEntranceHistories(employee_id, dtpWorkStart.Value, dtpWorkEnd.Value);
        }

        /// <summary>
        /// 追加ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAddForm_Click(object sender, EventArgs e)
        {
            var dlg = new AddHistoryForm();
            dlg.employee_id = cbemployee.SelectedValue.ToString();
            dlg.configuration = configuration;
            dlg.ShowDialog();
        }

        #endregion

        #region "関数"

        /// <summary>
        /// フォームを初期化
        /// </summary>
        private void initForm()
        {
            initEmployeesCombobox();
            initDatePicker();
        }

        /// <summary>
        /// DatePickerを初期化
        /// </summary>
        private void initDatePicker()
        {
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            dtpWorkStart.Value = firstDay.Date;

            dtpWorkEnd.Value = DateTime.Now.Date;
        }

        /// <summary>
        /// 従業員のコンボボックスを設定
        /// </summary>
        private void initEmployeesCombobox()
        {
            if (configuration == null)
            {
                return;
            }

            cbemployee.Items.Clear();
            cbemployee.DisplayMember = "Key";
            cbemployee.ValueMember = "Value";

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    employees
                WHERE
                    is_disabled = 0
                ORDER BY
                    id");
            if (dt == null)
            {
                return;
            }

            var dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                var name = dr["name"].ToString() ?? "";
                var id = dr["id"].ToString() ?? "";
                var display = $"【{id}】{name}";
                dic[display] = id;
            }

            cbemployee.DataSource = dic.ToList();
        }

        /// <summary>
        /// 履歴を表示
        /// </summary>
        private void searchEntranceHistories(string employee_id, DateTime start, DateTime end)
        {
            if (configuration == null)
            {
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object> {
                { "employee_id", employee_id },
                { "start", Utils.startTime(start) },
                { "end", Utils.endTime(end) },
            };
            var dt = db.ExecuteQuery(@"
                SELECT
                     entrance_histories.id
                    ,entrance_histories.enter_date
                    ,entrance_histories.leave_date
                    ,entrance_histories.memo
                FROM
                    entrance_histories
                INNER JOIN
                    employees
                ON
                    entrance_histories.employee_id = employees.id
                WHERE
                    employee_id = $employee_id
                AND
                    $start <= enter_date
                AND
                    enter_date <= $end
                ORDER BY
                    enter_date", param);
            if (dt == null)
            {
                dgvHistory.DataSource = null;
                return;
            }

            dgvHistory.DataSource = dt;
        }

        /// <summary>
        /// 勤怠履歴を削除
        /// </summary>
        /// <param name="attendance_id"></param>
        private void deleteAttendance(long attendance_id)
        {
            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object>() { { "id", attendance_id } };
            db.ExecuteNonQuery(@"
                DELETE FROM entrance_histories
                WHERE
                    id = $id
            ", param);
        }

        #endregion

    }
}
