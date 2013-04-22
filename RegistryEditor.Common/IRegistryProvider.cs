using Microsoft.Win32;

namespace RegistryEditor.Common
{
	public interface IRegistryProvider
	{
		RegistryKey GetRegistry(string machineName);
		RegistryKey GetRegistryAndOpenSubKey(string machineName, string subKey, bool writable = false);
		string FixupKeyName(RegistryKey key);
	}
}