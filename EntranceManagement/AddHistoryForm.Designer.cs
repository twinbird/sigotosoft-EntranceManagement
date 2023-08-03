namespace EntranceManagement
{
    partial class AddHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddHistoryForm));
            btnRegist = new Button();
            dtpEnterDate = new DateTimePicker();
            lblEnterDate = new Label();
            dtpLeaveDate = new DateTimePicker();
            lblLeaveDate = new Label();
            epInput = new ErrorProvider(components);
            cbemployee = new ComboBox();
            lblemployee = new Label();
            lblMemo = new Label();
            txtMemo = new TextBox();
            ((System.ComponentModel.ISupportInitialize)epInput).BeginInit();
            SuspendLayout();
            // 
            // btnRegist
            // 
            btnRegist.Location = new Point(151, 177);
            btnRegist.Name = "btnRegist";
            btnRegist.Size = new Size(75, 23);
            btnRegist.TabIndex = 3;
            btnRegist.Text = "登録";
            btnRegist.UseVisualStyleBackColor = true;
            btnRegist.Click += btnRegist_Click;
            // 
            // dtpEnterDate
            // 
            dtpEnterDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpEnterDate.Format = DateTimePickerFormat.Custom;
            dtpEnterDate.Location = new Point(80, 39);
            dtpEnterDate.Name = "dtpEnterDate";
            dtpEnterDate.Size = new Size(146, 23);
            dtpEnterDate.TabIndex = 1;
            // 
            // lblEnterDate
            // 
            lblEnterDate.AutoSize = true;
            lblEnterDate.Location = new Point(12, 43);
            lblEnterDate.Name = "lblEnterDate";
            lblEnterDate.Size = new Size(55, 15);
            lblEnterDate.TabIndex = 2;
            lblEnterDate.Text = "入室日時";
            // 
            // dtpLeaveDate
            // 
            dtpLeaveDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dtpLeaveDate.Format = DateTimePickerFormat.Custom;
            dtpLeaveDate.Location = new Point(80, 68);
            dtpLeaveDate.Name = "dtpLeaveDate";
            dtpLeaveDate.Size = new Size(146, 23);
            dtpLeaveDate.TabIndex = 2;
            // 
            // lblLeaveDate
            // 
            lblLeaveDate.AutoSize = true;
            lblLeaveDate.Location = new Point(12, 72);
            lblLeaveDate.Name = "lblLeaveDate";
            lblLeaveDate.Size = new Size(55, 15);
            lblLeaveDate.TabIndex = 2;
            lblLeaveDate.Text = "退出日時";
            // 
            // epInput
            // 
            epInput.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            epInput.ContainerControl = this;
            // 
            // cbemployee
            // 
            cbemployee.DropDownStyle = ComboBoxStyle.DropDownList;
            cbemployee.FormattingEnabled = true;
            cbemployee.Location = new Point(80, 10);
            cbemployee.Name = "cbemployee";
            cbemployee.Size = new Size(146, 23);
            cbemployee.TabIndex = 0;
            // 
            // lblemployee
            // 
            lblemployee.AutoSize = true;
            lblemployee.Location = new Point(14, 15);
            lblemployee.Name = "lblemployee";
            lblemployee.Size = new Size(43, 15);
            lblemployee.TabIndex = 5;
            lblemployee.Text = "従業員";
            // 
            // lblMemo
            // 
            lblMemo.AutoSize = true;
            lblMemo.Location = new Point(14, 96);
            lblMemo.Name = "lblMemo";
            lblMemo.Size = new Size(24, 15);
            lblMemo.TabIndex = 6;
            lblMemo.Text = "メモ";
            // 
            // txtMemo
            // 
            txtMemo.Location = new Point(80, 96);
            txtMemo.Multiline = true;
            txtMemo.Name = "txtMemo";
            txtMemo.Size = new Size(146, 68);
            txtMemo.TabIndex = 7;
            // 
            // AddHistoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(265, 212);
            Controls.Add(txtMemo);
            Controls.Add(lblMemo);
            Controls.Add(cbemployee);
            Controls.Add(lblemployee);
            Controls.Add(lblLeaveDate);
            Controls.Add(lblEnterDate);
            Controls.Add(dtpLeaveDate);
            Controls.Add(dtpEnterDate);
            Controls.Add(btnRegist);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AddHistoryForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しごとソフト【入退室管理】-履歴手動登録";
            Load += AddHistoryForm_Load;
            ((System.ComponentModel.ISupportInitialize)epInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnRegist;
        private DateTimePicker dtpEnterDate;
        private Label lblEnterDate;
        private DateTimePicker dtpLeaveDate;
        private Label lblLeaveDate;
        private ErrorProvider epInput;
        private ComboBox cbemployee;
        private Label lblemployee;
        private TextBox txtMemo;
        private Label lblMemo;
    }
}