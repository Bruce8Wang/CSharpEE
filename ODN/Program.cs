using System;
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
            //定义一个Button
            var btnSubmit = new Button();
            btnSubmit.Click += (sender, e) => { MessageBox.Show("Hello World !"); };
            //定义一个Form
            var frm = new Form();
            frm.Load += (sender, e) =>
            {
                var ScreenArea = Screen.GetWorkingArea(frm);
                frm.Location = new Point(0, 0);
                frm.Size = new Size(ScreenArea.Width, ScreenArea.Height);
                frm.Text = "主表单";
                btnSubmit.Location = new Point(ScreenArea.Width / 2 - btnSubmit.Size.Width / 2,
                    ScreenArea.Height / 2 - btnSubmit.Size.Height * 2);
                btnSubmit.Text = "Submit";
                frm.Controls.Add(btnSubmit);
            };
            //运行一个Form
            Application.Run(frm);
        }
    }
}