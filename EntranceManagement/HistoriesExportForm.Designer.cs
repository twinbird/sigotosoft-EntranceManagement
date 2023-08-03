namespace EntranceManagement
{
    partial class HistoriesExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoriesExportForm));
            gbSearchQuery = new GroupBox();
            dtpWorkEnd = new DateTimePicker();
            dtpWorkStart = new DateTimePicker();
            lblRange = new Label();
            lblRangeMark = new Label();
            btnOutputReport = new Button();
            btnExportExcel = new Button();
            btnExportCSV = new Button();
            gbSearchQuery.SuspendLayout();
            SuspendLayout();
            // 
            // gbSearchQuery
            // 
            gbSearchQuery.Controls.Add(dtpWorkEnd);
            gbSearchQuery.Controls.Add(dtpWorkStart);
            gbSearchQuery.Controls.Add(lblRange);
            gbSearchQuery.Controls.Add(lblRangeMark);
            gbSearchQuery.Location = new Point(12, 12);
            gbSearchQuery.Name = "gbSearchQuery";
            gbSearchQuery.Size = new Size(358, 59);
            gbSearchQuery.TabIndex = 0;
            gbSearchQuery.TabStop = false;
            gbSearchQuery.Text = "出力範囲";
            // 
            // dtpWorkEnd
            // 
            dtpWorkEnd.Location = new Point(220, 22);
            dtpWorkEnd.Name = "dtpWorkEnd";
            dtpWorkEnd.Size = new Size(124, 23);
            dtpWorkEnd.TabIndex = 1;
            // 
            // dtpWorkStart
            // 
            dtpWorkStart.Location = new Point(65, 22);
            dtpWorkStart.Name = "dtpWorkStart";
            dtpWorkStart.Size = new Size(124, 23);
            dtpWorkStart.TabIndex = 0;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Location = new Point(17, 26);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(43, 15);
            lblRange.TabIndex = 5;
            lblRange.Text = "入室日";
            // 
            // lblRangeMark
            // 
            lblRangeMark.AutoSize = true;
            lblRangeMark.Location = new Point(195, 27);
            lblRangeMark.Name = "lblRangeMark";
            lblRangeMark.Size = new Size(19, 15);
            lblRangeMark.TabIndex = 4;
            lblRangeMark.Text = "～";
            // 
            // btnOutputReport
            // 
            btnOutputReport.Location = new Point(295, 77);
            btnOutputReport.Name = "btnOutputReport";
            btnOutputReport.Size = new Size(75, 23);
            btnOutputReport.TabIndex = 1;
            btnOutputReport.Text = "帳票で出力";
            btnOutputReport.UseVisualStyleBackColor = true;
            btnOutputReport.Click += btnOutputReport_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(130, 77);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(86, 23);
            btnExportExcel.TabIndex = 1;
            btnExportExcel.Text = "Excelで出力";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // btnExportCSV
            // 
            btnExportCSV.Location = new Point(217, 77);
            btnExportCSV.Name = "btnExportCSV";
            btnExportCSV.Size = new Size(75, 23);
            btnExportCSV.TabIndex = 2;
            btnExportCSV.Text = "CSVで出力";
            btnExportCSV.UseVisualStyleBackColor = true;
            btnExportCSV.Click += btnExportCSV_Click;
            // 
            // HistoriesExportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(383, 111);
            Controls.Add(btnOutputReport);
            Controls.Add(gbSearchQuery);
            Controls.Add(btnExportExcel);
            Controls.Add(btnExportCSV);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "HistoriesExportForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しごとソフト【入退室管理】 - 記録の出力";
            Load += AttendanceExportForm_Load;
            gbSearchQuery.ResumeLayout(false);
            gbSearchQuery.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbSearchQuery;
        private DateTimePicker dtpWorkEnd;
        private DateTimePicker dtpWorkStart;
        private Label lblRange;
        private Label lblRangeMark;
        private Button btnExportCSV;
        private Button btnExportExcel;
        private Button btnOutputReport;
    }
}