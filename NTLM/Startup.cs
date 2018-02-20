using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Net;
using System.Web.Http;

namespace NTLM
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //设置使用Windows认证
            var listener = (HttpListener) app.Properties["System.Net.HttpListener"];
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
        }
    }
}