using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace EntranceManagement
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// �ݒ�t�@�C�����
        /// </summary>
        private ConfigWrapper mConfiguration;

        /// <summary>
        /// �A�v���̃o�[�W����
        /// </summary>
        const string APP_VERSION = "1.0";

        public MainForm()
        {
            InitializeComponent();

            mConfiguration = new ConfigWrapper("EntranceManagement.sqlite3");
        }

        #region "�C�x���g"

        /// <summary>
        /// �]�ƈ��ݒ�̃��j���[���N���b�N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �]�ƈ��ݒ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �]�ƈ��̓o�^�E�ҏW�t�H�[�����Ăяo��
            var dlg = new employeeManageForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();

            // �]�ƈ�����ǂݒ���
            loadEmployeesList();
        }

        /// <summary>
        /// ���ގ������̃��j���[���N���b�N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ���ގ�����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new HistoryForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();
        }

        /// <summary>
        /// �L�^�o�͂̃��j���[���N���b�N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �o��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new HistoriesExportForm();
            dlg.configuration = mConfiguration;
            dlg.ShowDialog();
        }

        /// <summary>
        /// �f�[�^�t�H���_���j���[���N���b�N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �f�[�^�t�H���_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = mConfiguration.getDBDirPath();
            System.Diagnostics.Process.Start("EXPLORER.EXE", path);
        }

        /// <summary>
        /// �����{�^�����N���b�N
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
        /// �ގ��{�^�����N���b�N
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
        /// ���C���t�H�[���̃��[�h�C�x���g
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
        /// �]�ƈ��̃��X�g��I��
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

        #region "�֐�"

        /// <summary>
        /// �f�[�^�x�[�X��ݒ�
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
        /// DDL�����s���ăe�[�u�����쐬
        /// </summary>
        private void execDDL()
        {
            var db = new SQLiteADOWrapper(mConfiguration.getDBFilePath());

            /* �A�v���o�[�W�����e�[�u�� */
            db.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS version (
	                 version TEXT PRIMARY KEY
	                ,created_at TEXT
	                ,updated_at TEXT
                )");

            /* �]�ƈ��e�[�u�� */
            db.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS employees (
	                 id TEXT PRIMARY KEY
	                ,name TEXT NOT NULL
                    ,memo TEXT
                    ,is_disabled INTEGER NOT NULL
	                ,created_at TEXT
	                ,updated_at TEXT
                )");

            /* ���ގ������e�[�u�� */
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
        /// DB�t�@�C���̃A�v���o�[�W�������擾
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
        /// �o�[�W���������X�V
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
        /// �t�H�[����������
        /// </summary>
        private void initForm()
        {
            initEmployeesListViewColumn();
            displayVersion();
        }

        /// <summary>
        /// �o�[�W��������\��
        /// </summary>
        private void displayVersion()
        {
            var dbver = getDatabaseVersion();
            var str = $"App Version {APP_VERSION}  DB Version {dbver}";
            lblVersion.Text = str;
        }

        /// <summary>
        /// �]�ƈ����̃��X�g�r���[������������
        /// </summary>
        private void initEmployeesListViewColumn()
        {
            // ���X�g�r���[�̃w�b�_�ݒ�
            lvemployees.Columns.Add("ID", 68, HorizontalAlignment.Left);
            lvemployees.Columns.Add("���O", 180, HorizontalAlignment.Left);
        }

        /// <summary>
        /// �]�ƈ��������X�g�r���[�֓ǂ݂���
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
        /// �Ō�ɓ��ގ��������ގ��������R�[�h��Ԃ�
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
        /// ���ގ��{�^���̖�����Ԃ�ύX
        /// </summary>
        private void changeEntranceButtonEnable()
        {
            var id = lvemployees.SelectedItems[0].Text;
            var latestRec = getLatestEntranceHistories(id);
            if (latestRec == null)
            {
                // �������Ȃ̂ŏo�΂݂̂ł���
                btnStartWork.Enabled = true;
                btnEndWork.Enabled = false;
                return;
            }

            // �ގ��ς݂Ȃ̂œ����݂̂ł���
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() != "")
            {
                btnStartWork.Enabled = true;
                btnEndWork.Enabled = false;
            }

            // �������Ă��鎞�ɂ͑ގ��݂̂ł���
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() == "")
            {
                btnStartWork.Enabled = false;
                btnEndWork.Enabled = true;
            }
        }

        /// <summary>
        /// ���ގ�������ݒ�
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
            // �ގ��ς݂Ȃ̂ŕ\�����Ȃ�
            if (latestRec["enter_date"].ToString() != "" && latestRec["leave_date"].ToString() != "")
            {
                txtMemo.Text = "";
                return;
            }

            txtMemo.Text = latestRec["memo"].ToString();
        }

        /// <summary>
        /// �����f�[�^��o�^
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
                MessageBox.Show("�o�^�Ɏ��s���܂����B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// �ގ��f�[�^��o�^
        /// </summary>
        private void registLeave(string id, string memo)
        {
            var rec = getLatestEntranceHistories(id);
            if (rec == null)
            {
                MessageBox.Show("�o�^�Ɏ��s���܂����B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var entrance_id = rec["id"].ToString();
            if (entrance_id == null)
            {
                MessageBox.Show("�o�^�Ɏ��s���܂����B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("�o�^�Ɏ��s���܂����B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

    #endregion

}