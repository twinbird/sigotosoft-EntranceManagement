using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntranceManagement
{
    public partial class AddHistoryForm : Form
    {
        #region "プロパティ"

        /// <summary>
        /// 設定ファイル情報のプロパティ
        /// </summary>
        public ConfigWrapper? configuration { get; set; }

        /// <summary>
        /// デフォルトの従業員ID
        /// </summary>
        public string? employee_id { get; set; }

        #endregion

        public AddHistoryForm()
        {
            InitializeComponent();
        }

        #region "イベント"

        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHistoryForm_Load(object sender, EventArgs e)
        {
            initEmployeesCombobox();
            cbemployee.SelectedValue = employee_id;
            setupDatePicker();
        }

        /// <summary>
        /// 登録ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (validateInput() == false)
            {
                return;
            }
            if (registAttendance() == true)
            {
                MessageBox.Show("登録しました", "登録完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region "関数"

        /// <summary>
        /// 日時指定用のDateTimePickerを初期化
        /// </summary>
        private void setupDatePicker()
        {
            var now = DateTime.Now;
            dtpEnterDate.Value = now;
            dtpLeaveDate.Value = now;
        }

        /// <summary>
        /// 検証エラーメッセージをリセットする
        /// </summary>
        private void resetValidateInput()
        {
            epInput.SetError(dtpEnterDate, null);
            epInput.SetError(dtpLeaveDate, null);
        }

        /// <summary>
        /// 入力検証
        /// </summary>
        /// <returns></returns>
        private bool validateInput()
        {
            resetValidateInput();
            var employee_id = cbemployee.SelectedValue.ToString() ?? "";

            // 入室日時が退室日時より後
            if (dtpEnterDate.Value > dtpLeaveDate.Value)
            {
                epInput.SetError(dtpEnterDate, "入室日時が退室日時より後になっています");
                return false;
            }

            // 入室日時か退室日時が登録済みの記録の範囲にある
            if (isIncludeRegistedAttendanceRange(employee_id, dtpEnterDate.Value))
            {
                epInput.SetError(dtpEnterDate, "入室日時が登録済みの入室時間内になっています");
                return false;
            }
            if (isIncludeRegistedAttendanceRange(employee_id, dtpLeaveDate.Value))
            {
                epInput.SetError(dtpLeaveDate, "退室日時が登録済みの入室時間内になっています");
                return false;
            }

            // 入室日時と退室日時の間に登録済みの履歴がある
            if (isIncludeRegistedAttendanceRange(employee_id, dtpEnterDate.Value, dtpLeaveDate.Value))
            {
                epInput.SetError(dtpEnterDate, "入室日時と退室日時の間に登録済みの履歴があります");
                return false;
            }

            return true;
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
        /// 履歴を登録する
        /// </summary>
        private bool registAttendance()
        {
            if (configuration == null)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var emp_id = cbemployee.SelectedValue.ToString();
            var start = dtpEnterDate.Value;
            var end = dtpLeaveDate.Value;
            var memo = txtMemo.Text;

            if (emp_id == null)
            {
                return false;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "employee_id", emp_id },
                { "enter_date", start },
                { "leave_date", end },
                { "memo", memo },
            };
            var ret = db.ExecuteNonQuery(@"
                INSERT INTO entrance_histories (
                     employee_id
                    ,enter_date
                    ,leave_date
                    ,memo
                    ,created_at
                    ,updated_at
                ) VALUES (
                     $employee_id
                    ,$enter_date
                    ,$leave_date
                    ,$memo
                    ,datetime('now', 'localtime')
                    ,datetime('now', 'localtime')
                )
                ", param);
            if (ret == -1)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 登録済みの勤務時間に引数の日時が含まれていればtrue
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool isIncludeRegistedAttendanceRange(string employee_id, DateTime dat)
        {
            if (configuration == null)
            {
                return false;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "employee_id", employee_id },
                { "dat1", dat },
                { "dat2", dat },
            };
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    entrance_histories
                WHERE
                    employee_id = $employee_id
                AND
                    enter_date <= $dat1
                AND
                    $dat2 <= coalesce(leave_date, datetime('9999-12-31 23:59:59'))
                ", param);
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 引数の日時に登録済みの勤務時間にが含まれていればtrue
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool isIncludeRegistedAttendanceRange(string employee_id, DateTime start, DateTime end)
        {
            if (configuration == null)
            {
                return false;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "employee_id", employee_id },
                { "enter_date", start },
                { "leave_date", end },
            };
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    entrance_histories
                WHERE
                    employee_id = $employee_id
                AND
                    $enter_date <= enter_date
                AND
                    leave_date <= $leave_date
                ", param);
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
