using System.Drawing;
using System.Windows.Forms;

namespace WindowsForms_Demo
{
    public sealed class FrmMain : Form
    {
        public FrmMain()
        {
            var btnSubmit = new Button
            {
                Location = new Point(250, 200),
                Size = new Size(75, 25),
                Name = "btnSubmit",
                Text = "Submit"
            };

            Load += (sender, e) =>
            {
                ClientSize = new Size(800, 600);
                Name = "frmMain";
                Text = "frmMain";
                Controls.Add(btnSubmit);
            };

            btnSubmit.Click += (sender, e) => 
			{ 
				MessageBox.Show("Hello World !"); 
			};
        }
    }
}