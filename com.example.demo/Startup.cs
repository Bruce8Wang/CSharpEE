using com.example.demo.Filters;
using com.example.demo.Models;
using com.example.demo.Providers;
using com.example.demo.Resolvers;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.StaticFiles;
using Owin;
using Swashbuckle.Application;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Extensions;
using Unity;
using Unity.Lifetime;

namespace com.example.demo
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // 3、启用oAuth2认证
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider("self"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                AllowInsecureHttp = true
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            #region "配置WebApi"
            var config = new HttpConfiguration();
            // 0、启用自定义路由
            config.MapHttpAttributeRoutes();
            // 1、启用OData
            config.AddODataQueryFilter();
            // 2、启用CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);
            // 4、启用Swagger
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "");
                c.IncludeXmlComments(String.Format("{0}/Swagger.XML", Environment.CurrentDirectory));
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<HttpHeaderFilter>();
            }).EnableSwaggerUi(c => { });
            // 7、启用依赖注入
            var container = new UnityContainer();
            container.RegisterType<BookDbContext, BookDbContext>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
            app.UseWebApi(config);
            #endregion
            // 5、启用静态资源文件映射
            var opt = new FileServerOptions
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(String.Format("{0}/Attachment", Environment.CurrentDirectory))
            };
            app.UseFileServer(opt);

            // 6、启用log4net
            XmlConfigurator.Configure();

            app.Run(context =>
            {
                context.Response.ContentType = "text/html";
                return context.Response.WriteAsync("Hello World !");
            });
        }
    }
}