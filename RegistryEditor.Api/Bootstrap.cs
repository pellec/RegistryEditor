using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using RegistryEditor.Common;
using RegistryEditor.Common.Configuration;
using RegistryEditor.Common.NetworkShare;

namespace RegistryEditor.Api
{
	public class Bootstrap
	{
		public void Configure(HttpConfiguration config)
		{
			IHttpRoute route;
			if (!config.Routes.TryGetValue("DefaultApi", out route))
			{
				config.Routes.MapHttpRoute(
					name: "DefaultApi",
					routeTemplate: "{controller}/{id}",
					defaults: new
						{
							controller = "Registry",
							id = RouteParameter.Optional
						}
					);
			}

			config.DependencyResolver = new AutofacWebApiDependencyResolver(ConfigureContainer());
			config.Filters.Add(new ValidateModelAttribute());
		}

		private static IContainer ConfigureContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<RegistryProvider>().As<IRegistryProvider>().SingleInstance();
			builder.RegisterType<ServerProvider>().As<IServerProvider>().SingleInstance();
			builder.RegisterType<NetworkShare>().As<INetworkShare>().SingleInstance();

			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			return builder.Build();
		}
	}
}