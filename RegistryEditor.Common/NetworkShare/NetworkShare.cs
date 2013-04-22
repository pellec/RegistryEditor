using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using RegistryEditor.Common.Configuration;

namespace RegistryEditor.Common.NetworkShare
{
	public class NetworkShare : INetworkShare
	{
		private readonly IServerProvider _serverProvider;
		private readonly ConcurrentDictionary<string, Server> _serverCache = new ConcurrentDictionary<string, Server>();

		internal struct NETRESOURCE
		{
			public int dwScope;
			public int dwType;
			public int dwDisplayType;
			public int dwUsage;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpLocalName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpRemoteName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpComment;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpProvider;
		}

		[DllImport("mpr.dll", EntryPoint = "WNetAddConnection2W", CharSet = CharSet.Unicode)]
		private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);

		[DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Unicode)]
		private static extern int WNetCancelConnection2(string serverName, Int32 dwFlags, bool fForce);

		private const int RESOURCETYPE_ANY = 0x0;

		public NetworkShare(IServerProvider serverProvider)
		{
			_serverProvider = serverProvider;
		}

		public bool TryConnect(string serverName)
		{
			Server server;
			if (!_serverProvider.TryGetServer(serverName, out server))
			{
				return false;
			}

			if (_serverCache.ContainsKey(server.Name))
			{
				return true;
			}

			var remoteName = GetRemoteName(serverName);
			var n = new NETRESOURCE
				{
					dwScope = 0,
					dwType = RESOURCETYPE_ANY,
					dwDisplayType = 0,
					dwUsage = 0,
					lpLocalName = null,
					lpRemoteName = remoteName,
					lpComment = null,
					lpProvider = null
				};

			var returnCode = WNetAddConnection2(ref n, server.Password, server.User, 0);
			if (returnCode == 1219 || returnCode <= 0)
			{
				_serverCache.TryAdd(server.Name, server);
				return true;
			}
			throw new Win32Exception(returnCode);
		}

		private string GetRemoteName(string serverName)
		{
			Server server;
			if(!_serverProvider.TryGetServer(serverName, out server))
			{
				throw new ApplicationException(string.Format("Could not find application configuration for server: '{0}'",
				                                             serverName));
			}

			return string.Format(@"\\{0}\{1}", serverName, server.Share);
		}

		public bool TryDisconnect(string serverName)
		{
			if(!_serverCache.ContainsKey(serverName))
			{
				return false;
			}

			try
			{
				var returnCode = WNetCancelConnection2(GetRemoteName(serverName), 0, true);
				if (returnCode != 0)
				{
					throw new Win32Exception(returnCode);
				}
			}
			finally
			{
				Server server;
				_serverCache.TryRemove(serverName, out server);
			}

			return true;
		}
	}
}