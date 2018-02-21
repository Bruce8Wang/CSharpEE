using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ODN
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //定义一个容器
            var container = new Container();
            //定义一个Timer
            var timer = new Timer(container);
            timer.Tick += (sender, e) =>
            {
                while (timer.Enabled)
                {
                    timer.Enabled = false;
                    MessageBox.Show("Hello Timer !");
                    timer.Enabled = true;
                }
            };
            //定义一个Button
            var btnSubmit = new Button();
            btnSubmit.Click += (sender, e) => { MessageBox.Show("Hello World !"); };
            //定义一个FlowLayout
            var flowLayout = new FlowLayoutPanel();
            //定义一个Form
            var frm = new Form();
            frm.Load += (sender, e) =>
            {
                var ScreenArea = Screen.GetWorkingArea(frm);
                //初始化Timer
                timer.Enabled = true;
                timer.Interval = 1000;
                //格式化Form
                frm.Location = new Point(ScreenArea.Width / 4, ScreenArea.Height / 4);
                frm.ClientSize = new Size(ScreenArea.Width / 2, ScreenArea.Height / 2);
                frm.Text = "主表单";
                //格式化Layout
                flowLayout.Size = new Size(frm.ClientSize.Width, frm.ClientSize.Height);
                //格式化Button
                btnSubmit.Size = new Size(75, 25);
                btnSubmit.Text = "Submit";
                //载入控件
                flowLayout.Controls.Add(btnSubmit);
                frm.Controls.Add(flowLayout);
            };
            //运行一个Form
            Application.Run(frm);
        }
    }
}