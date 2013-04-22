using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RegistryEditor.Common.Configuration
{
	public interface IServerProvider
	{
		bool TryGetServer(string serverName, out Server server);
		IEnumerable<Server> ListServers();
	}

	public class ServerProvider : IServerProvider
	{
		public ServerProvider()
		{
		}

		public bool TryGetServer(string serverName, out Server server)
		{
			server = null;
			AppSection appSection;
			if (!TryGetSection(out appSection))
			{
				return false;
			}

			server = appSection.Servers.Cast<Server>().FirstOrDefault(s => s.Name == serverName);
			return server != null;
		}

		public IEnumerable<Server> ListServers()
		{
			AppSection appSection;
			if (!TryGetSection(out appSection))
			{
				return Enumerable.Empty<Server>();
			}

			return appSection.Servers.Cast<Server>();

		}

		private static bool TryGetSection(out AppSection appSection)
		{
			appSection = ConfigurationManager.GetSection("app") as AppSection;
			return appSection != null;
		}
	}
}
