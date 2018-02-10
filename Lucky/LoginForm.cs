using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Lucky
{
    public class LoginForm : Form
    {
        private IContainer components = null;
        private Button btnset;
        private Label exitlabel;
        private Timer TitleTimer;
        private TextBox Titlefrontsizetext;
        private ComboBox cbSelectAward;
        private Button btnstart;
        bool restart = false;   //清空
        private static int acolor = 0;
        bool coloradd = true;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
        
        public LoginForm()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            components = new Container();
            var resources = new ComponentResourceManager(typeof(LoginForm));
            btnset = new Button();
            exitlabel = new Label();
            TitleTimer = new Timer(components);
            Titlefrontsizetext = new TextBox();
            cbSelectAward = new ComboBox();
            btnstart = new Button();
            SuspendLayout();
            // 
            // btnset
            // 
            btnset.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            btnset.AutoSize = true;
            btnset.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            btnset.BackgroundImageLayout = ImageLayout.Center;
            btnset.FlatAppearance.BorderSize = 0;
            btnset.FlatStyle = FlatStyle.Flat;
            btnset.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            btnset.ForeColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            btnset.Location = new Point(907, 490);
            btnset.Name = "btnset";
            btnset.Size = new Size(120, 41);
            btnset.TabIndex = 0;
            btnset.TabStop = false;
            btnset.Text = "奖项设置";
            btnset.TextAlign = ContentAlignment.BottomCenter;
            btnset.UseVisualStyleBackColor = false;
            btnset.Click += new EventHandler(btnset_Click);
            // 
            // exitlabel
            // 
            exitlabel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            exitlabel.AutoSize = true;
            exitlabel.BackColor = Color.Transparent;
            exitlabel.ForeColor = Color.Red;
            exitlabel.Location = new Point(10, 530);
            exitlabel.Name = "exitlabel";
            exitlabel.Size = new Size(77, 12);
            exitlabel.TabIndex = 3;
            exitlabel.Text = "按Alt+F4退出";
            // 
            // TitleTimer
            // 
            TitleTimer.Enabled = true;
            TitleTimer.Interval = 6;
            TitleTimer.Tick += new EventHandler(TitleTimer_Tick);
            // 
            // Titlefrontsizetext
            // 
            Titlefrontsizetext.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            Titlefrontsizetext.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            Titlefrontsizetext.BorderStyle = BorderStyle.FixedSingle;
            Titlefrontsizetext.Location = new Point(1032, 528);
            Titlefrontsizetext.Name = "Titlefrontsizetext";
            Titlefrontsizetext.Size = new Size(22, 21);
            Titlefrontsizetext.TabIndex = 8;
            Titlefrontsizetext.Text = "72";
            Titlefrontsizetext.DoubleClick += new EventHandler(Titlefrontsizetext_DoubleClick);
            // 
            // cbSelectAward
            // 
            cbSelectAward.Anchor = AnchorStyles.None;
            cbSelectAward.BackColor = Color.Red;
            cbSelectAward.FlatStyle = FlatStyle.Flat;
            cbSelectAward.Font = new Font("Microsoft YaHei", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            cbSelectAward.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            cbSelectAward.FormattingEnabled = true;
            cbSelectAward.ImeMode = ImeMode.On;
            cbSelectAward.Location = new Point(378, 346);
            cbSelectAward.Name = "cbSelectAward";
            cbSelectAward.RightToLeft = RightToLeft.No;
            cbSelectAward.Size = new Size(300, 60);
            cbSelectAward.TabIndex = 23;
            cbSelectAward.SelectedIndexChanged += new EventHandler(cbSelectAward_SelectedIndexChanged);
            // 
            // btnstart
            // 
            btnstart.Anchor = AnchorStyles.None;
            btnstart.AutoSize = true;
            btnstart.BackColor = Color.Transparent;
            btnstart.Cursor = Cursors.Hand;
            btnstart.FlatAppearance.BorderColor = Color.Red;
            btnstart.FlatAppearance.BorderSize = 0;
            btnstart.FlatAppearance.MouseDownBackColor = Color.Red;
            btnstart.FlatAppearance.MouseOverBackColor = Color.Red;
            btnstart.FlatStyle = FlatStyle.Flat;
            btnstart.Font = new Font("Microsoft YaHei", 60F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            btnstart.ForeColor = Color.Red;
            btnstart.Location = new Point(393, 94);
            btnstart.Margin = new Padding(0);
            btnstart.Name = "btnstart";
            btnstart.Size = new Size(260, 260);
            btnstart.TabIndex = 20;
            btnstart.UseVisualStyleBackColor = false;
            btnstart.Click += new EventHandler(btnstart_Click);
            // 
            // LoginForm
            // 
            AcceptButton = btnstart;
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1056, 551);
            Controls.Add(cbSelectAward);
            Controls.Add(Titlefrontsizetext);
            Controls.Add(exitlabel);
            Controls.Add(btnstart);
            Controls.Add(btnset);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "LoginForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormClosing += new FormClosingEventHandler(LoginForm_FormClosing);
            FormClosed += new FormClosedEventHandler(LoginForm_FormClosed);
            Load += new EventHandler(LoginForm_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        //系统设置
        private void btnset_Click(object sender, EventArgs e)
        {
            //SetForm set = new SetForm();
            var set = new frmSetForm();
            set.ShowDialog();

            if (set.DialogResult == DialogResult.OK)
            {
                BindAwardList();
            }

        }

        //进入抽奖
        private void btnstart_Click(object sender, EventArgs e)
        {
            var luckForm = new LuckyForm();
            luckForm.ShowDialog();

        }

        ////查看奖品
        //private void btnsee_Click(object sender, EventArgs e)
        //{
        //    ShowForm re = new ShowForm();
        //    re.ShowDialog();
        //}

        //加载
        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                BackgroundImage = Image.FromFile(ConfigurationManager.AppSettings["ResPath"] + @"\LoginForm.jpg");
                btnstart.BackgroundImage = Image.FromFile(ConfigurationManager.AppSettings["ResPath"] + @"\btnstart.jpg");
                UserHelper.ReadEmployeeList();
                BindAwardList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //绑定奖项
        private void BindAwardList()
        {
            var dt = UserHelper.ReadAward();
            UserHelper.DictAwardNum.Clear();
            cbSelectAward.Items.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbSelectAward.Items.Add(dt.Rows[i][0]);//读取奖项名称
                UserHelper.DictAwardNum.Add(dt.Rows[i][0].ToString(), Convert.ToInt32(dt.Rows[i][1])); //读取奖项设置数量
            }
            cbSelectAward.SelectedIndex = 0;
        }


        ////查看结果
        //private void btnresult_Click(object sender, EventArgs e)
        //{
        //    ResultForm re = new ResultForm();
        //    re.ShowDialog();
        //}

        //退出
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (restart == false)
            {
                var re = MessageBox.Show("您要退出抽奖系统吗?", "关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (re == DialogResult.Yes)
                {
                    var ree = MessageBox.Show("退出后本次抽奖记录将消失,确认要退出吗?", "重要提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (ree == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
            Dispose();
        }

        //清空
        private void btnrestart_Click(object sender, EventArgs e)
        {
            var re = MessageBox.Show("您确定要清空抽奖记录吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (re != DialogResult.OK) return;
            restart = true;
            Application.Restart();
        }

        private void TitleTimer_Tick(object sender, EventArgs e)
        {
            //TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(acolor)))));

            switch (acolor)
            {
                case 255:
                    coloradd = false;
                    break;
                case 0:
                    coloradd = true;
                    break;
            }

            if (coloradd)
            {
                acolor++;
            }
            else
            {
                acolor--;
            }
        }

        private void Titlefrontsizetext_DoubleClick(object sender, EventArgs e)
        {
            var tmpsize = 72;
            try
            {
                tmpsize = int.Parse(Titlefrontsizetext.Text.Trim());
            }
            catch (Exception)
            {
                return;
            }
            //TitleLabel.Font = new System.Drawing.Font("华文新魏", tmpsize);
        }

        private void cbSelectAward_SelectedIndexChanged(object sender, EventArgs e)
        {
            //保存选择的奖项
            UserHelper.awardName = cbSelectAward.SelectedItem.ToString();

            //选择奖项后，向Dictionary中增加一个数组，用于存放已中奖人员
            if (UserHelper.DictAwardWinner.ContainsKey(UserHelper.awardName) == false)
                UserHelper.DictAwardWinner.Add(UserHelper.awardName, new List<string[]>());

            //读取单项奖已中奖人员名单
            UserHelper.ReadEmployeeListWinner(UserHelper.awardName);
        }
    }
}