using System.Web;
using System.Web.SessionState;

namespace demo
{
    public class App: HttpApplication
    {
        public override void Init()
        {
            PostAuthenticateRequest += (sender, e) =>
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            };
            base.Init();
        }
    }
}