using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace EntranceManagement
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 設定ファイル情報
        /// </summary>
        private ConfigWrapper mConfiguration;

        /// <summary>
        /// アプリのバージョン
        /// </summary>
        const string APP_VERSION = "1.0";

        public MainForm()
        {
            InitializeComponent();

            mConfiguration = new ConfigWrapper("EntranceManagement.sqlite3");
        }

        #region "イベント"

        /// <summary>
        /// 従業員設定のメニューをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 従業員設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 従業員の登録・編集フォームを呼び出す
            var dlg = new employeeManageForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();

            // 従業員情報を読み直す
            loadEmployeesList();
        }

        /// <summary>
        /// 入退室履歴のメニューをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 入退室履歴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new HistoryForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();
        }

        /// <summary>
        /// 記録出力のメニューをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 出力ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new HistoriesExportForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();
        }

        /// <summary>
        /// データフォルダメニューをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void データフォルダToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = mConfiguration.getDBDirPath();
            System.Diagnostics.Process.Start("EXPLORER.EXE", path);
        }

        /// <summary>
        /// 入室ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnter_Click(object sender, EventArgs e)
        {
            var id = lvemployees.SelectedItems[0].Text;
            var memo = txtMemo.Text;
            registEnter(id, memo);
            changeEntranceButtonEnable();
        }

        /// <summary>
        /// 退室ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeave_Click(object sender, EventArgs e)
        {
            var id = lvemployees.SelectedItems[0].Text;
            var memo = txtMemo.Text;
            registLeave(id, memo);
            changeEntranceButtonEnable();

            txtMemo.Text = "";
        }

        /// <summary>
        /// メインフォームのロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            setupDatabase();
            initForm();
            loadEmployeesList();
        }

        /// <summary>
        /// 従業員のリストを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvemployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvemployees.SelectedIndices.Count == 0)
            {
                return;
            }
            changeEntranceButtonEnable();
            setMemo();
        }

        #endregion

        #region "関数"

        /// <summary>
        /// データベースを設定
        /// </summary>
        private void setupDatabase()
        {
            execDDL();
            if (getDatabaseVersion() == "")
            {
                insertVersionRecord();
            }
        }

        /// <summary>
        /// DDLを実行してテーブルを作成
        /// </summary>
        private void execDDL()
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());

            /* アプリバージョンテーブル */
            db.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS version (
	                 version TEXT PRIMARY KEY
	                ,created_at TEXT
	                ,updated_at TEXT
                )");

            /* 従業員テーブル */
            db.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS employees (
	                 id TEXT PRIMARY KEY
	                ,name TEXT NOT NULL
                    ,memo TEXT
                    ,is_disabled INTEGER NOT NULL
	                ,created_at TEXT
	                ,updated_at TEXT
                )");

            /* 入退室履歴テーブル */
            db.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS entrance_histories (
                     id INTEGER PRIMARY KEY AUTOINCREMENT
	                ,employee_id TEXT NOT NULL
	                ,enter_date TEXT NOT NULL
	                ,leave_date TEXT
                    ,memo TEXT
	                ,created_at TEXT
	                ,updated_at TEXT
                )");
        }

        /// <summary>
        /// DBファイルのアプリバージョンを取得
        /// </summary>
        private string getDatabaseVersion()
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());
            var dt = db.ExecuteQuery(@"
                SELECT
                    version
                FROM
                    version
            ");
            if (dt == null || dt.Rows.Count == 0)
            {
                return "";
            }
            return dt.Rows[0]["version"].ToString() ?? "";
        }

        /// <summary>
        /// バージョン情報を更新
        /// </summary>
        private void insertVersionRecord()
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());

            var param = new Dictionary<string, object>() { { "version", APP_VERSION } };
            db.ExecuteNonQuery(@"
                INSERT INTO version (version, created_at, updated_at)
                VALUES ($version, datetime('now', 'localtime'), datetime('now', 'localtime'))
            ", param);

        }

        /// <summary>
        /// フォームを初期化
        /// </summary>
        private void initForm()
        {
            initEmployeesListViewColumn();
            displayVersion();
        }

        /// <summary>
        /// バージョン情報を表示
        /// </summary>
        private void displayVersion()
        {
            var dbver = getDatabaseVersion();
            var str = $"App Version {APP_VERSION}  DB Version {dbver}";
            lblVersion.Text = str;
        }

        /// <summary>
        /// 従業員情報のリストビューを初期化する
        /// </summary>
        private void initEmployeesListViewColumn()
        {
            // リストビューのヘッダ設定
            lvemployees.Columns.Add("ID", 68, HorizontalAlignment.Left);
            lvemployees.Columns.Add("名前", 180, HorizontalAlignment.Left);
        }

        /// <summary>
        /// 従業員情報をリストビューへ読みだす
        /// </summary>
        private void loadEmployeesList()
        {
            lvemployees.Items.Clear();

            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());
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

            foreach (DataRow dr in dt.Rows)
            {
                var row = lvemployees.Items.Add(dr["id"].ToString());
                row.SubItems.Add(dr["name"].ToString());
            }
        }

        /// <summary>
        /// 最後に入退室した入退室履歴レコードを返す
        /// </summary>
        /// <param name="employee_id"></param>
        /// <returns></returns>
        private DataRow? getLatestEntranceHistories(string employee_id)
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());
            var param = new Dictionary<string, object>() { { "id", employee_id } };
            var dt = db.ExecuteQuery(@"
                SELECT
                    *
                FROM
                    entrance_histories
                WHERE
                    employee_id = $id
                ORDER BY
                     enter_date DESC
                LIMIT 1", param);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            return dt.Rows[0];
        }

        /// <summary>
        /// 入退室ボタンの無効状態を変更
        /// </summary>
        private void changeEntranceButtonEnable()
        {
            var id = lvemployees.SelectedItems[0].Text;
            var latestRec = getLatestEntranceHistories(id);
            if (latestRec == null)
            {
                // 未入室なので出勤のみできる
                btnStartWork.Enabled = true;
                btnEndWork.Enabled = false;
                return;
            }

            // 退室済みなので入室のみできる
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() != "")
            {
                btnStartWork.Enabled = true;
                btnEndWork.Enabled = false;
            }

            // 入室している時には退室のみできる
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() == "")
            {
                btnStartWork.Enabled = false;
                btnEndWork.Enabled = true;
            }
        }

        /// <summary>
        /// 入退室メモを設定
        /// </summary>
        private void setMemo()
        {
            var id = lvemployees.SelectedItems[0].Text;
            var latestRec = getLatestEntranceHistories(id);
            if (latestRec == null)
            {
                txtMemo.Text = "";
                return;
            }
            // 退室済みなので表示しない
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() != "")
            {
                txtMemo.Text = "";
                return;
            }

            txtMemo.Text = latestRec["memo"].ToString();
        }

        /// <summary>
        /// 入室データを登録
        /// </summary>
        private void registEnter(string id, string memo)
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "employee_id", id },
                { "memo", memo },
            };
            var ret = db.ExecuteNonQuery(@"
                INSERT INTO entrance_histories (
                     employee_id
                    ,enter_date
                    ,memo
                    ,created_at
                    ,updated_at
                ) VALUES (
                     $employee_id
                    ,datetime('now', 'localtime')
                    ,$memo
                    ,datetime('now', 'localtime')
                    ,datetime('now', 'localtime')
                )
                ", param);
            if (ret == -1)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 退室データを登録
        /// </summary>
        private void registLeave(string id, string memo)
        {
            var rec = getLatestEntranceHistories(id);
            if (rec == null)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var entrance_id = rec["id"].ToString();
            if (entrance_id == null)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());
            var param = new Dictionary<string, object>() {
                { "employee_id", id },
                { "id", entrance_id },
                { "memo", memo },
            };
            var ret = db.ExecuteNonQuery(@"
                UPDATE entrance_histories
                SET
                     leave_date = datetime('now', 'localtime')
                    ,memo = $memo
                    ,updated_at =  datetime('now', 'localtime')
                WHERE
                    id = $id
                AND
                    employee_id = $employee_id
                ", param);
            if (ret == -1)
            {
                MessageBox.Show("登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

    #endregion

}