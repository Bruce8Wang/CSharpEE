using log4net.Config;
using Owin;

namespace GTA.PI.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //设置使用Windows认证
            //HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
            //listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;

            OAuthConfig.Register(app);
            WebApiConfig.Register(app);
            XmlConfigurator.Configure();

            //设置首页
            app.Run(context =>
            {
                context.Response.ContentType = "text/html";
                return context.Response.WriteAsync("<meta http-equiv='refresh' content='0; url =/swagger' />");
            });
        }
    }
}
