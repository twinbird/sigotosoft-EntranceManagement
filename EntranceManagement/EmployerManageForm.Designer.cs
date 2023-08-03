namespace EntranceManagement
{
    partial class employeeManageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(employeeManageForm));
            txtID = new TextBox();
            lblID = new Label();
            txtName = new TextBox();
            lblName = new Label();
            gbemployee = new GroupBox();
            txtMemo = new TextBox();
            cbDisabled = new CheckBox();
            btnDelete = new Button();
            btnRegist = new Button();
            lblMemo = new Label();
            gbDataManage = new GroupBox();
            btnCSVImport = new Button();
            btnCSVExport = new Button();
            lvemployees = new ListView();
            epemployeeInfo = new ErrorProvider(components);
            gbemployee.SuspendLayout();
            gbDataManage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)epemployeeInfo).BeginInit();
            SuspendLayout();
            // 
            // txtID
            // 
            txtID.Location = new Point(72, 27);
            txtID.MaxLength = 10;
            txtID.Name = "txtID";
            txtID.Size = new Size(179, 23);
            txtID.TabIndex = 0;
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Location = new Point(5, 30);
            lblID.Name = "lblID";
            lblID.Size = new Size(54, 15);
            lblID.TabIndex = 3;
            lblID.Text = "従業員ID";
            // 
            // txtName
            // 
            txtName.Location = new Point(72, 56);
            txtName.MaxLength = 50;
            txtName.Name = "txtName";
            txtName.Size = new Size(179, 23);
            txtName.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(5, 59);
            lblName.Name = "lblName";
            lblName.Size = new Size(31, 15);
            lblName.TabIndex = 3;
            lblName.Text = "名前";
            // 
            // gbemployee
            // 
            gbemployee.Controls.Add(txtMemo);
            gbemployee.Controls.Add(cbDisabled);
            gbemployee.Controls.Add(btnDelete);
            gbemployee.Controls.Add(btnRegist);
            gbemployee.Controls.Add(txtName);
            gbemployee.Controls.Add(lblMemo);
            gbemployee.Controls.Add(lblName);
            gbemployee.Controls.Add(txtID);
            gbemployee.Controls.Add(lblID);
            gbemployee.Location = new Point(271, 12);
            gbemployee.Name = "gbemployee";
            gbemployee.Size = new Size(278, 220);
            gbemployee.TabIndex = 1;
            gbemployee.TabStop = false;
            gbemployee.Text = "従業員情報";
            // 
            // txtMemo
            // 
            txtMemo.Location = new Point(72, 85);
            txtMemo.MaxLength = 1000;
            txtMemo.Multiline = true;
            txtMemo.Name = "txtMemo";
            txtMemo.Size = new Size(179, 70);
            txtMemo.TabIndex = 2;
            // 
            // cbDisabled
            // 
            cbDisabled.AutoSize = true;
            cbDisabled.Location = new Point(86, 161);
            cbDisabled.Name = "cbDisabled";
            cbDisabled.Size = new Size(159, 19);
            cbDisabled.TabIndex = 3;
            cbDisabled.Text = "無効にする(退職・休職など)";
            cbDisabled.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(13, 192);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(76, 23);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "削除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRegist
            // 
            btnRegist.Location = new Point(172, 192);
            btnRegist.Name = "btnRegist";
            btnRegist.Size = new Size(93, 23);
            btnRegist.TabIndex = 4;
            btnRegist.Text = "登録";
            btnRegist.UseVisualStyleBackColor = true;
            btnRegist.Click += btnRegist_Click;
            // 
            // lblMemo
            // 
            lblMemo.AutoSize = true;
            lblMemo.Location = new Point(5, 85);
            lblMemo.Name = "lblMemo";
            lblMemo.Size = new Size(24, 15);
            lblMemo.TabIndex = 3;
            lblMemo.Text = "メモ";
            // 
            // gbDataManage
            // 
            gbDataManage.Controls.Add(btnCSVImport);
            gbDataManage.Controls.Add(btnCSVExport);
            gbDataManage.Location = new Point(271, 300);
            gbDataManage.Name = "gbDataManage";
            gbDataManage.Size = new Size(278, 102);
            gbDataManage.TabIndex = 2;
            gbDataManage.TabStop = false;
            gbDataManage.Text = "データの管理";
            // 
            // btnCSVImport
            // 
            btnCSVImport.Location = new Point(35, 59);
            btnCSVImport.Name = "btnCSVImport";
            btnCSVImport.Size = new Size(206, 23);
            btnCSVImport.TabIndex = 1;
            btnCSVImport.Text = "CSVからインポート";
            btnCSVImport.UseVisualStyleBackColor = true;
            btnCSVImport.Click += btnCSVImport_Click;
            // 
            // btnCSVExport
            // 
            btnCSVExport.Location = new Point(35, 30);
            btnCSVExport.Name = "btnCSVExport";
            btnCSVExport.Size = new Size(206, 23);
            btnCSVExport.TabIndex = 0;
            btnCSVExport.Text = "CSVでエクスポート";
            btnCSVExport.UseVisualStyleBackColor = true;
            btnCSVExport.Click += btnCSVExport_Click;
            // 
            // lvemployees
            // 
            lvemployees.FullRowSelect = true;
            lvemployees.GridLines = true;
            lvemployees.Location = new Point(12, 16);
            lvemployees.MultiSelect = false;
            lvemployees.Name = "lvemployees";
            lvemployees.Size = new Size(253, 390);
            lvemployees.TabIndex = 0;
            lvemployees.UseCompatibleStateImageBehavior = false;
            lvemployees.View = View.Details;
            lvemployees.SelectedIndexChanged += lvemployees_SelectedIndexChanged;
            // 
            // epemployeeInfo
            // 
            epemployeeInfo.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            epemployeeInfo.ContainerControl = this;
            // 
            // employeeManageForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(554, 414);
            Controls.Add(lvemployees);
            Controls.Add(gbDataManage);
            Controls.Add(gbemployee);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "employeeManageForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しごとソフト【入退室管理】-従業員の登録・編集";
            Load += employeeManageForm_Load;
            gbemployee.ResumeLayout(false);
            gbemployee.PerformLayout();
            gbDataManage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)epemployeeInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtID;
        private Label lblID;
        private TextBox txtName;
        private Label lblName;
        private GroupBox gbemployee;
        private CheckBox cbDisabled;
        private Button btnRegist;
        private Button btnDelete;
        private GroupBox gbDataManage;
        private Button btnCSVImport;
        private Button btnCSVExport;
        private Label lblMemo;
        private TextBox txtMemo;
        private ListView lvemployees;
        private ErrorProvider epemployeeInfo;
    }
}