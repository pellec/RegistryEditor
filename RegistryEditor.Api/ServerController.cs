using System.Net;
using System.Net.Http;
using System.Web.Http;
using RegistryEditor.Common.Configuration;

namespace RegistryEditor.Api
{
	public class ServerController : ApiController
	{
		private readonly IServerProvider _serverProvider;

		public ServerController(IServerProvider serverProvider)
		{
			_serverProvider = serverProvider;
		}

		public HttpResponseMessage Get()
		{
			var servers = _serverProvider.ListServers();

			return Request.CreateResponse(HttpStatusCode.OK, servers);
		}
	}
}
