using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using RegistryEditor.Api;

namespace RegistryEditor.IntegrationTests
{
	public class HttpClientFactory
	{
		public HttpClient Create()
		{
			var baseAddress = new Uri("http://localhost:9999");
			var config = new HttpSelfHostConfiguration(baseAddress);
			new Bootstrap().Configure(config);
			var server = new HttpSelfHostServer(config);
			var client = new HttpClient(server);
			try
			{
				client.BaseAddress = baseAddress;
				return client;
			}
			catch 
			{
				client.Dispose();
				throw;
			}
		}
	}
}