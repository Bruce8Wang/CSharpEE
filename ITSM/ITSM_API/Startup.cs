using System.Net;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using Owin;

namespace ITSM
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			//设置使用Windows认证
			HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
			listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;

			HttpConfiguration config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.AddODataQueryFilter();
			app.UseWebApi(config);

			app.Run(context =>
			{
				context.Response.ContentType = "text/html";
				return context.Response.WriteAsync("Hello World !");
			});
		}
	}
}
