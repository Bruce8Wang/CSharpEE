using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Extensions;

namespace BookService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(Register);
        }

        public static void Register(HttpConfiguration config)
        {
            // Enabling Cross-Origin Requests   
            config.AddODataQueryFilter();

            //启用CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
