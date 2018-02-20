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
                btnSubmit.Location = new Point(120, 80);
                btnSubmit.Size = new Size(75, 25);
                btnSubmit.Text = "Submit";
                frm.ClientSize = new Size(400, 300);
                frm.Text = "主表单";
                frm.Controls.Add(btnSubmit);
            };
            //运行一个Form
            Application.Run(frm);
        }
    }
}