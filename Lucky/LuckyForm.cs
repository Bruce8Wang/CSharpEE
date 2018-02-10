using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lucky
{
    public class LuckyForm : Form
    {
        private IContainer components = null;
        private Button btnOnoff;
        private Label lblname;
        private Label lblLevelName;
        private ImageList ilface;
        private Timer tmrchoose;
        private Button btnbc;
        private Label lblcc;
        private Label lbln;
        private Label lblset;
        private Label label2;
        private Button btnRestart;
        private Label lblCenter;
        private Label lblEmpNo;
        private ImageList imageList1;
        private TextBox txtNum;
        private Button button2;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
        public int choose;
        readonly int pernum = 10;                 //总人数(图片的数量)
        readonly int i = 0;                  //图片下标(总人数)

        //抽奖随机取出的人员列表
        IList<string[]> il;
        int winSingle = 0;          //单项抽出的中奖人数
        private const int winSum = 0; //已抽出的总中奖人数
        int singleNum = 0;           //单屏抽出人数
        string currentWinner = "";    //中奖者下标
        string currentCenter = "";
        string currentEmpNo = "";

        bool go = true;             //判断开始按钮  
        private Size beforeResizeSize = Size.Empty;
        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            beforeResizeSize = Size;
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
        }

        public LuckyForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            components = new Container();
            var resources = new ComponentResourceManager(typeof(LuckyForm));
            btnOnoff = new Button();
            lblname = new Label();
            lblLevelName = new Label();
            ilface = new ImageList(components);
            tmrchoose = new Timer(components);
            btnbc = new Button();
            lblcc = new Label();
            lbln = new Label();
            lblset = new Label();
            label2 = new Label();
            btnRestart = new Button();
            lblCenter = new Label();
            lblEmpNo = new Label();
            imageList1 = new ImageList(components);
            txtNum = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // btnOnoff
            // 
            btnOnoff.Anchor = AnchorStyles.Bottom;
            btnOnoff.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            btnOnoff.Cursor = Cursors.Hand;
            btnOnoff.FlatAppearance.BorderSize = 0;
            btnOnoff.FlatAppearance.MouseDownBackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            btnOnoff.FlatStyle = FlatStyle.Flat;
            btnOnoff.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            btnOnoff.ForeColor = Color.Maroon;
            btnOnoff.Location = new Point(339, 666);
            btnOnoff.Name = "btnOnoff";
            btnOnoff.Size = new Size(140, 45);
            btnOnoff.TabIndex = 1;
            btnOnoff.TabStop = false;
            btnOnoff.Text = "开始";
            btnOnoff.UseVisualStyleBackColor = false;
            btnOnoff.Click += new EventHandler(btnonoff_Click);
            // 
            // lblname
            // 
            lblname.BackColor = Color.Transparent;
            lblname.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblname.ForeColor = Color.Yellow;
            lblname.Location = new Point(250, 110);
            lblname.Name = "lblname";
            lblname.Size = new Size(600, 700);
            lblname.TabIndex = 3;
            lblname.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLevelName
            // 
            lblLevelName.Anchor = AnchorStyles.Top;
            lblLevelName.BackColor = Color.Transparent;
            lblLevelName.Font = new Font("Microsoft YaHei", 45F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblLevelName.ForeColor = Color.Yellow;
            lblLevelName.Location = new Point(142, 9);
            lblLevelName.Name = "lblLevelName";
            lblLevelName.Size = new Size(800, 100);
            lblLevelName.TabIndex = 3;
            lblLevelName.Text = "一等奖";
            lblLevelName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ilface
            // 
            ilface.ColorDepth = ColorDepth.Depth16Bit;
            ilface.ImageSize = new Size(200, 220);
            ilface.TransparentColor = Color.Transparent;
            // 
            // tmrchoose
            // 
            tmrchoose.Interval = 5;
            tmrchoose.Tick += new EventHandler(tmrchoose_Tick);
            // 
            // btnbc
            // 
            btnbc.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            btnbc.BackColor = Color.OrangeRed;
            btnbc.Cursor = Cursors.Hand;
            btnbc.FlatAppearance.BorderSize = 0;
            btnbc.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnbc.FlatStyle = FlatStyle.Flat;
            btnbc.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            btnbc.ForeColor = Color.Maroon;
            btnbc.Location = new Point(12, 666);
            btnbc.Name = "btnbc";
            btnbc.Size = new Size(139, 44);
            btnbc.TabIndex = 5;
            btnbc.TabStop = false;
            btnbc.Text = "返回";
            btnbc.UseVisualStyleBackColor = false;
            btnbc.Click += new EventHandler(btnbc_Click);
            // 
            // lblcc
            // 
            lblcc.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            lblcc.BackColor = Color.Transparent;
            lblcc.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblcc.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            lblcc.Location = new Point(887, 675);
            lblcc.Name = "lblcc";
            lblcc.Size = new Size(150, 34);
            lblcc.TabIndex = 6;
            lblcc.Text = "0";
            // 
            // lbln
            // 
            lbln.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            lbln.AutoSize = true;
            lbln.BackColor = Color.Transparent;
            lbln.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            lbln.ForeColor = Color.Yellow;
            lbln.Location = new Point(789, 672);
            lbln.Name = "lbln";
            lbln.Size = new Size(104, 36);
            lbln.TabIndex = 7;
            lbln.Text = "已抽出:";
            // 
            // lblset
            // 
            lblset.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            lblset.BackColor = Color.Transparent;
            lblset.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblset.ForeColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            lblset.Location = new Point(706, 673);
            lblset.Name = "lblset";
            lblset.Size = new Size(73, 34);
            lblset.TabIndex = 6;
            lblset.Text = "0";
            // 
            // label2
            // 
            label2.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            label2.ForeColor = Color.Yellow;
            label2.Location = new Point(633, 671);
            label2.Name = "label2";
            label2.Size = new Size(77, 36);
            label2.TabIndex = 7;
            label2.Text = "设置:";
            // 
            // btnRestart
            // 
            btnRestart.Anchor = AnchorStyles.Bottom;
            btnRestart.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            btnRestart.Cursor = Cursors.Hand;
            btnRestart.FlatAppearance.BorderSize = 0;
            btnRestart.FlatAppearance.MouseDownBackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            btnRestart.ForeColor = Color.Maroon;
            btnRestart.Location = new Point(496, 666);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(140, 45);
            btnRestart.TabIndex = 2;
            btnRestart.TabStop = false;
            btnRestart.Text = "重抽";
            btnRestart.UseVisualStyleBackColor = false;
            btnRestart.Visible = false;
            btnRestart.Click += new EventHandler(btnRestart_Click);
            // 
            // lblCenter
            // 
            lblCenter.Anchor = AnchorStyles.Top;
            lblCenter.BackColor = Color.Transparent;
            lblCenter.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblCenter.ForeColor = Color.Yellow;
            lblCenter.Location = new Point(400, 110);
            lblCenter.Name = "lblCenter";
            lblCenter.Size = new Size(500, 700);
            lblCenter.TabIndex = 8;
            lblCenter.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEmpNo
            // 
            lblEmpNo.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            lblEmpNo.BackColor = Color.Transparent;
            lblEmpNo.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lblEmpNo.ForeColor = Color.Yellow;
            lblEmpNo.Location = new Point(650, 110);
            lblEmpNo.Name = "lblEmpNo";
            lblEmpNo.Size = new Size(298, 700);
            lblEmpNo.TabIndex = 9;
            lblEmpNo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // txtNum
            // 
            txtNum.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            txtNum.BackColor = Color.Red;
            txtNum.BorderStyle = BorderStyle.None;
            txtNum.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            txtNum.ForeColor = Color.Yellow;
            txtNum.Location = new Point(259, 671);
            txtNum.Name = "txtNum";
            txtNum.Size = new Size(56, 36);
            txtNum.TabIndex = 27;
            txtNum.Text = "10";
            // 
            // button2
            // 
            button2.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            button2.BackColor = Color.Transparent;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseDownBackColor = Color.Maroon;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Microsoft YaHei", 20.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            button2.ForeColor = Color.Gold;
            button2.Location = new Point(157, 666);
            button2.Name = "button2";
            button2.Size = new Size(111, 44);
            button2.TabIndex = 28;
            button2.Text = "人数:";
            button2.UseVisualStyleBackColor = false;
            // 
            // LuckyForm
            // 
            AcceptButton = btnOnoff;
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1008, 730);
            Controls.Add(txtNum);
            Controls.Add(lblcc);
            Controls.Add(lblset);
            Controls.Add(lbln);
            Controls.Add(label2);
            Controls.Add(btnRestart);
            Controls.Add(btnOnoff);
            Controls.Add(button2);
            Controls.Add(btnbc);
            Controls.Add(lblEmpNo);
            Controls.Add(lblCenter);
            Controls.Add(lblname);
            Controls.Add(lblLevelName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "LuckyForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "抽奖系统";
            WindowState = FormWindowState.Maximized;
            FormClosing += new FormClosingEventHandler(LuckyForm_FormClosing);
            Load += new EventHandler(LuckyForm_Load);
            ResumeLayout(false);
            PerformLayout();

        }

        private void LuckyForm_Load(object sender, EventArgs e)
        {
            btnOnoff.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\btnOnOff.jpg");
            btnbc.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\btnbc.jpg");
            btnRestart.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\btnRestart.jpg");
            BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\LuckyForm.jpg");

            //奖项名称
            lblLevelName.Text = UserHelper.awardName;
            //设置数量
            lblset.Text = UserHelper.DictAwardNum[UserHelper.awardName].ToString();
            //已抽数量
            winSingle = UserHelper.DictAwardWinner[UserHelper.awardName].Count;
            lblcc.Text = winSingle.ToString();
            switch (UserHelper.awardName)
            {
                case "董事长特别奖":
                case "总裁特别奖":
                    txtNum.Text = "1";
                    break;
                case "宜居生活奖":
                    txtNum.Text = "1";
                    break;
                case "特等奖":
                    txtNum.Text = "5";
                    break;
                case "挑战梦想奖":
                    txtNum.Text = "5";
                    break;
                case "一等奖":
                case "二等奖":
                    txtNum.Text = "10";
                    break;
                case "三等奖":
                    txtNum.Text = "15";
                    break;
            }
        }

        //读取抽取的名额
        private int readnum(int num)
        {
            var re = 0;  //计数器
            for (var i = 0; i < UserHelper.DictAwardWinner[UserHelper.awardName].Count; i++)
            {
                if (UserHelper.DictAwardWinner[UserHelper.awardName][i] != null)
                {
                    re++;
                }
                else
                {
                    break;
                }
            }

            return re;
        }

        //图片游动控制
        private void tmrchoose_Tick(object sender, EventArgs e)
        {
            //取出当前抽奖人数
            singleNum = Convert.ToInt32(txtNum.Text);

            il = getListItems(UserHelper.sname, singleNum);
            lblname.Text = "\n";
            lblCenter.Text = "\n";
            lblEmpNo.Text = "\n";

            //循环显示中奖人员信息
            foreach (var t in il)
            {
                lblname.Text = lblname.Text + t[0].ToString() + "\n" + "\n";
                lblCenter.Text = lblCenter.Text + t[1].ToString() + "\n" + "\n";
                lblEmpNo.Text = lblEmpNo.Text + t[2].ToString() + "\n" + "\n";
            }
        }

        //点击开始抽奖
        private void btnonoff_Click(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(txtNum.Text))
            {
                case 5:
                    lblname.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblCenter.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblEmpNo.Font = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    break;
                case 10:
                    lblname.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblCenter.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblEmpNo.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

                    break;
                case 15:
                    lblname.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblCenter.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblEmpNo.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    break;
                case 20:
                    lblname.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblCenter.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblEmpNo.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    break;
                default:
                    lblname.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblCenter.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lblEmpNo.Font = new Font("Microsoft YaHei", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    break;
            }

            //图片张数大于1
            if (pernum > 0)
            {
                //判断按键开始抽奖
                if (go)
                {
                    //当抽出名额大于设置名额时提示返回
                    if (Convert.ToInt32(lblcc.Text) >= Convert.ToInt32(lblset.Text))
                    {
                        DialogResult re = MessageBox.Show("您设置的中奖人数已经全部抽出,要返回吗?", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (re == DialogResult.OK)
                        {
                            Close();
                        }
                        else
                        {
                            tmrchoose.Enabled = true;
                            btnOnoff.Text = "停止";
                            go = false;
                        }
                    }
                    else
                    {
                        tmrchoose.Enabled = true;
                        btnOnoff.Text = "停止";
                        go = false;
                    }
                }
                else
                {
                    if (btnOnoff.Text.Equals("保存"))
                    {

                        foreach (var t in il)
                        {
                            UserHelper.DictAwardWinner[UserHelper.awardName].Add(t);
                            UserHelper.sname.Remove(t);
                            winSingle = winSingle + 1; //单项中奖人数自增一
                            lblcc.Text = winSingle.ToString();
                        }
                        UserHelper.RemoveWinner(il);
                        UserHelper.SaveToExcel(UserHelper.DictAwardWinner[UserHelper.awardName], winSingle - singleNum, UserHelper.awardName, singleNum);

                        btnOnoff.Text = "开始";
                        btnRestart.Visible = false;
                        go = true;

                        currentWinner = "";//清空中奖者缓存
                    }
                    else
                    {

                        tmrchoose.Enabled = false;
                        bool ishave;//是否重复   
                        do
                        {
                            ishave = false;      //初始化

                            //如果己抽出的人数大于参加抽奖的人数,充许中两次奖(一般不可能出现~~!)
                            if (winSum >= i - 1)
                            {
                                //ishave = false;
                                break;
                            }

                        } while (ishave);

                        SetLucky();

                        lblname.Text = currentWinner;
                        lblCenter.Text = currentCenter;
                        lblEmpNo.Text = currentEmpNo;

                        btnOnoff.Text = "保存";
                        btnRestart.Visible = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("没有找到抽奖成员!", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //?
        private void SetLucky()
        {
            #region 显示中奖人员

            if (!string.IsNullOrEmpty(currentWinner)) return;
            currentWinner = "\n";
            currentCenter = "\n";
            currentEmpNo = "\n";

            //循环显示中奖人员信息
            foreach (var t in il)
            {
                currentWinner = currentWinner + t[0].ToString() + "\n" + "\n";
                currentCenter = currentCenter + t[1].ToString() + "\n" + "\n";
                currentEmpNo = currentEmpNo + t[2].ToString() + "\n" + "\n";
            }
            #endregion
        }

        //返回
        private void btnbc_Click(object sender, EventArgs e)
        {
            Close();
        }

        //退出关闭tmr
        private void LuckyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnOnoff.Text.Equals("保存"))
            {
                var re = MessageBox.Show("已抽出中奖者但尚未保存,要关闭吗?", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (re == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            GC.Collect();
            Dispose();
        }

        #region 随机取出若干个不重复的项

        private IList<string[]> getListItems(IList<string[]> list, int num)
        {
            //如果剩余人数不足以选出num个，则num=剩余所有人数
            if (num > list.Count) num = list.Count;

            //新建一个泛型列表,将传入的列表复制过来,用于运算,而不要直接操作传入的列表;  
            IList<string[]> temp_list = new List<string[]>(list);

            //取出的项,保存在此列表
            IList<string[]> return_list = new List<string[]>();

            Random random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < num; i++)
            {
                //判断如果列表还有可以取出的项,以防下标越界
                if (temp_list.Count > 0)
                {
                    //在列表中产生一个随机索引
                    int arrIndex = random.Next(0, temp_list.Count);
                    //将此随机索引的对应的列表元素值复制出来
                    return_list.Add(temp_list[arrIndex]);
                    //然后删掉此索引的列表项
                    temp_list.RemoveAt(arrIndex);
                }
                else
                {
                    //列表项取完后,退出循环,比如列表本来只有10项,但要求取出20项.
                    break;
                }
            }

            return return_list;

        }
        #endregion

        private void btnRestart_Click(object sender, EventArgs e)
        {
            tmrchoose.Enabled = true;
            btnOnoff.Text = "停止";
            go = false;
            btnRestart.Visible = false;
            currentWinner = "";//清空中奖者缓存
        }

    }
}