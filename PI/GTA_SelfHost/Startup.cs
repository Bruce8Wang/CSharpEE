using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System.Configuration;

namespace GTA.SelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            FileServerOptions opt = new FileServerOptions
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(ConfigurationManager.AppSettings["root"])
            };
            app.UseFileServer(opt);
        }
    }
}
