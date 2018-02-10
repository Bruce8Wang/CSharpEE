using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Net;
using System.Web.Http;

namespace com.example.demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //设置使用Windows认证
            HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            var opt = new FileServerOptions
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(string.Format("{0}/static", Environment.CurrentDirectory))
            };
            app.UseFileServer(opt);

            app.Run(context =>
            {
                context.Response.ContentType = "text/html";
                return context.Response.WriteAsync("Hello World !");
            });
        }
    }
}
