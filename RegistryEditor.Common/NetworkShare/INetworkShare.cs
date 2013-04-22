namespace RegistryEditor.Common.NetworkShare
{
	public interface INetworkShare
	{
		bool TryDisconnect(string serverName);

		bool TryConnect(string serverName);
	}
}