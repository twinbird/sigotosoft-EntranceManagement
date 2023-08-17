using Csv;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace EntranceManagement
{
    /// <summary>
    /// 従業員の登録・編集フォーム
    /// </summary>
    public partial class employeeManageForm : Form
    {
        #region "プロパティ"

        /// <summary>
        /// 設定ファイル情報のプロパティ
        /// </summary>
        public ConfigWrapper? configuration { get; set; }

        #endregion

        #region "定数"

        /// <summary>
        /// 従業員情報のリストビューの新規追加用行の名前の値
        /// </summary>
        const string employeeS_NEW_USER_ROW_NAME_VALUE = "<新規追加>";

        /// <summary>
        /// 従業員情報のリストビューの新規追加用行のIDの値
        /// </summary>
        const string employeeS_NEW_USER_ROW_ID_VALUE = "";

        #endregion

        #region "コンストラクタ"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public employeeManageForm()
        {
            InitializeComponent();
        }

        #endregion

        #region "イベント"

        /// <summary>
        /// 登録・更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (lvemployees.SelectedItems.Count == 0)
            {
                return;
            }
            var id = lvemployees.SelectedItems[0].Text;

            // 入力検証
            if (validateemployeeInfo(id) == false)
            {
                return;
            }

            var param = new Dictionary<string, object>();
            param["id"] = txtID.Text;
            param["name"] = txtName.Text;
            param["memo"] = txtMemo.Text;
            param["is_disabled"] = cbDisabled.Checked ? 1 : 0;

            if (id == employeeS_NEW_USER_ROW_ID_VALUE)
            {
                insertemployeeInfo(param);
            }
            else
            {
                updateemployeeInfo(id, param);
            }

            MessageBox.Show("登録しました", "メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // リストビュー更新
            loademployeesList();
        }

        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvemployees.SelectedItems.Count == 0)
            {
                return;
            }
            var id = lvemployees.SelectedItems[0].Text;
            if (id == employeeS_NEW_USER_ROW_ID_VALUE)
            {
                return;
            }

            if (MessageBox.Show("本当に削除してもよろしいですか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            deleteemployee(id);
            resetInputForm();
            loademployeesList();
        }

        /// <summary>
        /// フォームロード時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void employeeManageForm_Load(object sender, EventArgs e)
        {
            initemployeesListViewColumn();
            loademployeesList();
        }

        /// <summary>
        /// リストビューを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvemployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvemployees.SelectedItems.Count == 0)
            {
                return;
            }
            var id = lvemployees.SelectedItems[0];
            loademployeeInfo(id.Text);
        }

        /// <summary>
        /// CSVエクスポートボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSVExport_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = "従業員情報.csv";
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.Title = "保存先を指定してください。";
            sfd.FilterIndex = 2;
            sfd.Filter = "CSVファイル(*.csv)|*.csv";

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            exportCSV(sfd.FileName);
        }

        /// <summary>
        /// CSVインポートボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSVImport_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.DefaultExt = ".csv";
            ofd.Title = "インポートするファイルを指定してください";
            ofd.FilterIndex = 2;
            ofd.Filter = "CSVファイル(*.csv)|*.csv";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (MessageBox.Show("従業員IDが登録済みのものは上書きされます。\n従業員IDの列が空の行は無視されます。\n本当にインポートしますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            importCSV(ofd.FileName);
            loademployeesList();
        }

        #endregion

        #region "関数"

        /// <summary>
        /// 従業員情報のリストビューを初期化する
        /// </summary>
        private void initemployeesListViewColumn()
        {
            // リストビューのヘッダ設定
            lvemployees.Columns.Add("ID", 68, HorizontalAlignment.Left);
            lvemployees.Columns.Add("名前", 180, HorizontalAlignment.Left);
        }

        /// <summary>
        /// 従業員情報をリストビューへ読みだす
        /// </summary>
        private void loademployeesList()
        {
            lvemployees.Items.Clear();

            // 新規追加用の行
            lvemployees.Items.Add(employeeS_NEW_USER_ROW_ID_VALUE).SubItems.Add(employeeS_NEW_USER_ROW_NAME_VALUE);

            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    employees
                ORDER BY
                    id");
            if (dt == null)
            {
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                var row = lvemployees.Items.Add(dr["id"].ToString());
                row.SubItems.Add(dr["name"].ToString());
            }

            // 新規追加用の行をアクティブにしておく
            lvemployees.Items[0].Selected = true;
        }

        /// <summary>
        /// 入力フォームをクリアする
        /// </summary>
        private void resetInputForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtMemo.Text = "";
            cbDisabled.Checked = false;
            txtID.Enabled = true;

            resetValidateemployeeInfo();
        }

        /// <summary>
        /// リストビューで選択されている従業員情報を入力欄へ表示する
        /// </summary>
        private void loademployeeInfo(string id)
        {
            resetInputForm();

            if (id == employeeS_NEW_USER_ROW_ID_VALUE)
            {
                return;
            }

            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());

            var param = new Dictionary<string, object> { { "id", id } };
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    employees
                WHERE
                    id = $id", param);
            if (dt == null)
            {
                return;
            }

            txtID.Text = dt.Rows[0]["id"].ToString();
            txtName.Text = dt.Rows[0]["name"].ToString();
            txtMemo.Text = dt.Rows[0]["memo"].ToString();
            if ((long)dt.Rows[0]["is_disabled"] == 1)
            {
                cbDisabled.Checked = true;
            }
            txtID.Enabled = false;
        }

        /// <summary>
        /// 入力フォームの内容を登録する
        /// </summary>
        /// <param name="param"></param>
        private void insertemployeeInfo(Dictionary<string, object> param)
        {
            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            db.ExecuteNonQuery(@"
                INSERT INTO employees (
                     id
                    ,name
                    ,memo
                    ,is_disabled
                    ,created_at
                    ,updated_at
                ) VALUES (
                     $id
                    ,$name
                    ,$memo
                    ,$is_disabled
                    ,datetime('now', 'localtime')
                    ,datetime('now', 'localtime')
                )
            ", param);
        }

        /// <summary>
        /// 入力フォームの内容で更新する
        /// </summary>
        /// <param name="param"></param>
        private void updateemployeeInfo(string id, Dictionary<string, object> param)
        {
            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            param.Add("old_id", id);
            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            db.ExecuteNonQuery(@"
                UPDATE employees
                SET
                     id = $id
                    ,name = $name
                    ,memo = $memo
                    ,is_disabled = $is_disabled
                    ,updated_at = datetime('now', 'localtime')
                WHERE
                    id = $old_id
            ", param);
        }

        /// <summary>
        /// 登録済みの従業員IDならtrue
        /// </summary>
        /// <param name="id"></param>
        private bool isExistemployeeID(string id)
        {
            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            var param = new Dictionary<string, object>() { { "id", id } };
            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    employees
                WHERE
                    id = $id", param);
            if (dt == null)
            {
                return true;
            }

            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 従業員IDの検証
        /// </summary>
        /// <param name="current_id"></param>
        /// <returns></returns>
        private bool validateemployeeID(string current_id)
        {
            // ID未入力はエラー
            if (txtID.Text == "")
            {
                epemployeeInfo.SetError(txtID, "従業員IDは必ず入力してください");
                return false;
            }

            // ID登録済みならエラー: 新規の場合
            if (current_id == employeeS_NEW_USER_ROW_ID_VALUE && isExistemployeeID(txtID.Text))
            {
                epemployeeInfo.SetError(txtID, "すでに登録されている従業員IDです。");
                return false;
            }
            // ID登録済みならエラー: 更新の場合(ID変更の場合)
            if (current_id != employeeS_NEW_USER_ROW_ID_VALUE &&
                current_id != txtID.Text &&
                isExistemployeeID(txtID.Text))
            {
                epemployeeInfo.SetError(txtID, "すでに登録されている従業員IDです。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 従業員名の検証
        /// </summary>
        /// <param name="current_id"></param>
        /// <returns></returns>
        private bool validateemployeeName()
        {
            // 名前未入力はエラー
            if (txtName.Text == "")
            {
                epemployeeInfo.SetError(txtName, "名前は必ず入力してください");
                return false;
            }
            return true;
        }

        /// <summary>
        /// エラー表示をリセットする
        /// </summary>
        private void resetValidateemployeeInfo()
        {
            epemployeeInfo.SetError(txtID, null);
            epemployeeInfo.SetError(txtName, null);
        }

        /// <summary>
        /// 従業員入力フォームの検証
        /// </summary>
        /// <param name="current_id"></param>
        /// <returns></returns>
        private bool validateemployeeInfo(string current_id)
        {
            // 一度クリア
            resetValidateemployeeInfo();

            var ret = true;

            // 従業員ID
            if (validateemployeeID(current_id) == false)
            {
                ret = false;
            }

            // 名前
            if (validateemployeeName() == false)
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// 選択中の従業員を削除する
        /// </summary>
        /// <param name="id"></param>
        private void deleteemployee(string id)
        {
            if (id == employeeS_NEW_USER_ROW_ID_VALUE)
            {
                return;
            }

            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());

            var param = new Dictionary<string, object> { { "id", id } };
            db.ExecuteNonQuery(@"
                DELETE
                FROM
                    employees
                WHERE
                    id = $id", param);
        }

        /// <summary>
        /// 従業員情報をCSVでエクスポートする
        /// </summary>
        private void exportCSV(string filepath)
        {
            if (configuration == null)
            {
                MessageBox.Show("configuration is not set", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var db = new SQLiteADOWrapper(configuration.getDBFilePath());
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    employees
                ORDER BY
                    id");
            if (dt == null)
            {
                return;
            }

            var columnNames = new[] { "従業員ID", "名前", "メモ", "無効" };
            var rowsList = new List<string[]>();
            foreach (DataRow dr in dt.Rows)
            {
                var row = new[]
                {
                    dr["id"].ToString() ?? "",
                    dr["name"].ToString() ?? "",
                    dr["memo"].ToString() ?? "",
                    dr["is_disabled"].ToString() ?? "",
                };
                rowsList.Add(row);
            }
            var rows = rowsList;
            var csv = CsvWriter.WriteToText(columnNames, rows, ',');

            // Shift-JISを使えるようにする
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            File.WriteAllText(filepath, csv, Encoding.GetEncoding("shift_jis"));
        }

        /// <summary>
        /// CSVファイルを読みだしてディクショナリのリストにして返す
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private List<Dictionary<string, string>> csv2Dict(string filepath)
        {
            // Shift-JISを使えるようにする
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var csv = File.ReadAllText(filepath, Encoding.GetEncoding("shift_jis"));
            var ret = new List<Dictionary<string, string>>();
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                var dict = new Dictionary<string, string>();
                dict["id"] = line[0];
                dict["name"] = line[1];
                dict["memo"] = line[2];
                dict["is_disabled"] = line[3];

                ret.Add(dict);
            }
            return ret;
        }

        /// <summary>
        /// CSVファイル1行の内容を検証する
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        private bool validateImportemployee(Dictionary<string, string> employee)
        {
            if (employee == null) { return false; }

            // IDが11文字以上
            if (employee["id"].Length > 10)
            {
                return false;
            }

            // 名前が空
            if (employee["name"].Length == 0)
            {
                return false;
            }
            // 名前が51文字以上
            if (employee["name"].Length > 50)
            {
                return false;
            }

            // メモが1000文字以上
            if (employee["memo"].Length > 1000)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// CSVファイル1行の内容を登録する
        /// </summary>
        /// <param name="employee"></param>
        private void insertImportemployee(Dictionary<string, string> employee)
        {
            var emp = new Dictionary<string, object>();
            emp["id"] = employee["id"];
            emp["name"] = employee["name"];
            emp["memo"] = employee["memo"];
            emp["is_disabled"] = employee["is_disabled"] == "1" ? 1 : 0;
            insertemployeeInfo(emp);
        }

        /// <summary>
        /// CSVファイル1行の内容を更新する
        /// </summary>
        /// <param name="employee"></param>
        private void updateImportemployee(Dictionary<string, string> employee)
        {
            var emp = new Dictionary<string, object>();
            emp["id"] = employee["id"];
            emp["name"] = employee["name"];
            emp["memo"] = employee["memo"];
            emp["is_disabled"] = employee["is_disabled"] == "1" ? 1 : 0;

            var id = employee["id"];
            updateemployeeInfo(id, emp);
        }

        /// <summary>
        /// 従業員情報をCSVからインポートする
        /// </summary>
        /// <param name="filepath"></param>
        private void importCSV(string filepath)
        {
            var dat = csv2Dict(filepath);
            var errorList = new List<string>();
            foreach (var row in dat)
            {
                if (row["id"] == "")
                {
                    continue;
                }
                if (validateImportemployee(row) == false)
                {
                    errorList.Add("従業員ID: " + row["id"] + "は不正な入力のためスキップされました。\n");
                    continue;
                }
                if (isExistemployeeID(row["id"]))
                {
                    updateImportemployee(row);
                }
                else
                {
                    insertImportemployee(row);
                }
            }

            if (errorList.Count > 0)
            {
                var message = "";
                foreach (var error in errorList)
                {
                    message += error;
                }

                MessageBox.Show(message);
            }
        }

        #endregion

    }
}
