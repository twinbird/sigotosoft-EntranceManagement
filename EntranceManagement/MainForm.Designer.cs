namespace EntranceManagement
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnStartWork = new Button();
            btnEndWork = new Button();
            menuMain = new MenuStrip();
            従業員設定ToolStripMenuItem = new ToolStripMenuItem();
            入退室履歴ToolStripMenuItem = new ToolStripMenuItem();
            出力ToolStripMenuItem = new ToolStripMenuItem();
            データフォルダToolStripMenuItem = new ToolStripMenuItem();
            lvemployees = new ListView();
            lblVersion = new Label();
            lblMemo = new Label();
            txtMemo = new TextBox();
            menuMain.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartWork
            // 
            btnStartWork.Enabled = false;
            btnStartWork.Font = new Font("Yu Gothic UI", 48F, FontStyle.Bold, GraphicsUnit.Point);
            btnStartWork.ForeColor = Color.Green;
            btnStartWork.Location = new Point(287, 29);
            btnStartWork.Name = "btnStartWork";
            btnStartWork.Size = new Size(403, 101);
            btnStartWork.TabIndex = 1;
            btnStartWork.Text = "入室";
            btnStartWork.UseVisualStyleBackColor = true;
            btnStartWork.Click += btnEnter_Click;
            // 
            // btnEndWork
            // 
            btnEndWork.Enabled = false;
            btnEndWork.Font = new Font("Yu Gothic UI", 48F, FontStyle.Bold, GraphicsUnit.Point);
            btnEndWork.ForeColor = Color.Red;
            btnEndWork.Location = new Point(287, 136);
            btnEndWork.Name = "btnEndWork";
            btnEndWork.Size = new Size(403, 101);
            btnEndWork.TabIndex = 2;
            btnEndWork.Text = "退室";
            btnEndWork.UseVisualStyleBackColor = true;
            btnEndWork.Click += btnLeave_Click;
            // 
            // menuMain
            // 
            menuMain.Items.AddRange(new ToolStripItem[] { 従業員設定ToolStripMenuItem, 入退室履歴ToolStripMenuItem, 出力ToolStripMenuItem, データフォルダToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(704, 24);
            menuMain.TabIndex = 6;
            menuMain.Text = "メニュー";
            // 
            // 従業員設定ToolStripMenuItem
            // 
            従業員設定ToolStripMenuItem.Name = "従業員設定ToolStripMenuItem";
            従業員設定ToolStripMenuItem.Size = new Size(109, 20);
            従業員設定ToolStripMenuItem.Text = "従業員登録・編集";
            従業員設定ToolStripMenuItem.Click += 従業員設定ToolStripMenuItem_Click;
            // 
            // 入退室履歴ToolStripMenuItem
            // 
            入退室履歴ToolStripMenuItem.Name = "入退室履歴ToolStripMenuItem";
            入退室履歴ToolStripMenuItem.Size = new Size(79, 20);
            入退室履歴ToolStripMenuItem.Text = "入退室履歴";
            入退室履歴ToolStripMenuItem.Click += 入退室履歴ToolStripMenuItem_Click;
            // 
            // 出力ToolStripMenuItem
            // 
            出力ToolStripMenuItem.Name = "出力ToolStripMenuItem";
            出力ToolStripMenuItem.Size = new Size(67, 20);
            出力ToolStripMenuItem.Text = "記録出力";
            出力ToolStripMenuItem.Click += 出力ToolStripMenuItem_Click;
            // 
            // データフォルダToolStripMenuItem
            // 
            データフォルダToolStripMenuItem.Name = "データフォルダToolStripMenuItem";
            データフォルダToolStripMenuItem.Size = new Size(80, 20);
            データフォルダToolStripMenuItem.Text = "データフォルダ";
            データフォルダToolStripMenuItem.Click += データフォルダToolStripMenuItem_Click;
            // 
            // lvemployees
            // 
            lvemployees.BorderStyle = BorderStyle.FixedSingle;
            lvemployees.FullRowSelect = true;
            lvemployees.GridLines = true;
            lvemployees.Location = new Point(12, 29);
            lvemployees.MultiSelect = false;
            lvemployees.Name = "lvemployees";
            lvemployees.Size = new Size(252, 371);
            lvemployees.TabIndex = 7;
            lvemployees.UseCompatibleStateImageBehavior = false;
            lvemployees.View = View.Details;
            lvemployees.SelectedIndexChanged += lvemployees_SelectedIndexChanged;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(509, 403);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(141, 15);
            lblVersion.TabIndex = 8;
            lblVersion.Text = "App Version:  DB Version: ";
            // 
            // lblMemo
            // 
            lblMemo.AutoSize = true;
            lblMemo.Location = new Point(287, 240);
            lblMemo.Name = "lblMemo";
            lblMemo.Size = new Size(72, 15);
            lblMemo.TabIndex = 9;
            lblMemo.Text = "入退出時メモ";
            // 
            // txtMemo
            // 
            txtMemo.Location = new Point(287, 258);
            txtMemo.Multiline = true;
            txtMemo.Name = "txtMemo";
            txtMemo.Size = new Size(403, 131);
            txtMemo.TabIndex = 10;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(704, 424);
            Controls.Add(txtMemo);
            Controls.Add(lblMemo);
            Controls.Add(lblVersion);
            Controls.Add(lvemployees);
            Controls.Add(btnEndWork);
            Controls.Add(btnStartWork);
            Controls.Add(menuMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuMain;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しごとソフト【入退室管理】";
            Load += MainForm_Load;
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnStartWork;
        private Button btnEndWork;
        private MenuStrip menuMain;
        private ToolStripMenuItem 従業員設定ToolStripMenuItem;
        private ToolStripMenuItem 入退室履歴ToolStripMenuItem;
        private ToolStripMenuItem 出力ToolStripMenuItem;
        private ListView lvemployees;
        private Label lblVersion;
        private ToolStripMenuItem データフォルダToolStripMenuItem;
        private Label lblMemo;
        private TextBox txtMemo;
    }
}