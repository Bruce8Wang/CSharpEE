namespace SyncDataJob
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpSync;
        private System.Windows.Forms.RichTextBox rtbResult;
        private System.Windows.Forms.Timer timerJob;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnBasic;
        private System.Windows.Forms.Button btnDataByTime;
        private System.Windows.Forms.Button btnFinance;
        private System.Windows.Forms.Button btnPlateSymbol;
        private System.Windows.Forms.Button btnTechnical;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.dtpSync = new System.Windows.Forms.DateTimePicker();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.timerJob = new System.Windows.Forms.Timer(this.components);
            this.btnBasic = new System.Windows.Forms.Button();
            this.btnDataByTime = new System.Windows.Forms.Button();
            this.btnFinance = new System.Windows.Forms.Button();
            this.btnPlateSymbol = new System.Windows.Forms.Button();
            this.btnTechnical = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(26, 13);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(65, 12);
            this.lblStartTime.TabIndex = 0;
            this.lblStartTime.Text = "开始时间：";
            // 
            // dtpSync
            // 
            this.dtpSync.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpSync.Location = new System.Drawing.Point(98, 9);
            this.dtpSync.Name = "dtpSync";
            this.dtpSync.Size = new System.Drawing.Size(93, 21);
            this.dtpSync.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(198, 10);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "一键同步";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // rtbResult
            // 
            this.rtbResult.Location = new System.Drawing.Point(28, 45);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.Size = new System.Drawing.Size(668, 432);
            this.rtbResult.TabIndex = 99;
            this.rtbResult.Text = "";
            // 
            // timerJob
            // 
            this.timerJob.Interval = 1000;
            this.timerJob.Tick += new System.EventHandler(this.timerJob_Tick);
            // 
            // btnBasic
            // 
            this.btnBasic.Location = new System.Drawing.Point(297, 10);
            this.btnBasic.Name = "btnBasic";
            this.btnBasic.Size = new System.Drawing.Size(75, 23);
            this.btnBasic.TabIndex = 3;
            this.btnBasic.Text = "基础信息";
            this.btnBasic.UseVisualStyleBackColor = true;
            this.btnBasic.Click += new System.EventHandler(this.btnBasic_Click);
            // 
            // btnDataByTime
            // 
            this.btnDataByTime.Location = new System.Drawing.Point(459, 9);
            this.btnDataByTime.Name = "btnDataByTime";
            this.btnDataByTime.Size = new System.Drawing.Size(75, 23);
            this.btnDataByTime.TabIndex = 5;
            this.btnDataByTime.Text = "行情指标";
            this.btnDataByTime.UseVisualStyleBackColor = true;
            this.btnDataByTime.Click += new System.EventHandler(this.btnDataByTime_Click);
            // 
            // btnFinance
            // 
            this.btnFinance.Location = new System.Drawing.Point(540, 9);
            this.btnFinance.Name = "btnFinance";
            this.btnFinance.Size = new System.Drawing.Size(75, 23);
            this.btnFinance.TabIndex = 6;
            this.btnFinance.Text = "财务指标";
            this.btnFinance.UseVisualStyleBackColor = true;
            this.btnFinance.Click += new System.EventHandler(this.btnFinance_Click);
            // 
            // btnPlateSymbol
            // 
            this.btnPlateSymbol.Location = new System.Drawing.Point(378, 10);
            this.btnPlateSymbol.Name = "btnPlateSymbol";
            this.btnPlateSymbol.Size = new System.Drawing.Size(75, 23);
            this.btnPlateSymbol.TabIndex = 4;
            this.btnPlateSymbol.Text = "板块指标";
            this.btnPlateSymbol.UseVisualStyleBackColor = true;
            this.btnPlateSymbol.Click += new System.EventHandler(this.btnPlateSymbol_Click);
            // 
            // btnTechnical
            // 
            this.btnTechnical.Location = new System.Drawing.Point(621, 10);
            this.btnTechnical.Name = "btnTechnical";
            this.btnTechnical.Size = new System.Drawing.Size(75, 23);
            this.btnTechnical.TabIndex = 8;
            this.btnTechnical.Text = "技术指标";
            this.btnTechnical.UseVisualStyleBackColor = true;
            this.btnTechnical.Click += new System.EventHandler(this.btnTechnical_Click);
            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(725, 504);
            this.Controls.Add(this.btnTechnical);
            this.Controls.Add(this.btnPlateSymbol);
            this.Controls.Add(this.btnFinance);
            this.Controls.Add(this.btnDataByTime);
            this.Controls.Add(this.btnBasic);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.rtbResult);
            this.Controls.Add(this.dtpSync);
            this.Controls.Add(this.lblStartTime);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}