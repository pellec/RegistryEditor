using System.Web.Http;
using RegistryEditor.Api;

namespace RegistryEditor.Web.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
			  name: "DefaultApi",
			  routeTemplate: "api/{controller}/{id}",
			  defaults: new { id = RouteParameter.Optional }
			  );

			new Bootstrap().Configure(config);

			config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
		}
	}
}