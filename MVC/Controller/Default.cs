using demo.Model;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace demo.Controller
{
    public class Default : Page
    {
        protected Button btnSubmit;
        protected Label lblShow;

        protected override void OnInit(EventArgs f)
        {
            Load += (sender, e) =>
            {
                if (IsPostBack)
                {
                    Response.Write("我爱你，中国！");
                }
            };
            btnSubmit.Click += (sender, e) =>
            {
                var o = new Person();
                o.Id = 1;
                o.Name = "Bruce";
                lblShow.Text = "Hello " + o.Name;
            };
        }
    }
}