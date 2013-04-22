using Microsoft.Win32;

namespace RegistryEditor.Common
{
	public class RegistryProvider : IRegistryProvider
	{
		private const string RegistryHive = "HKEY_LOCAL_MACHINE\\";
		private const string RegistryRoot = "software";
		private const string RegistryKeySeperator = "\\";

		public RegistryKey GetRegistry(string machineName)
		{
			return RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, machineName);
		}

		public RegistryKey GetRegistryAndOpenSubKey(string machineName, string subKey, bool writable = false)
		{
			return GetRegistry(machineName).OpenSubKey((RegistryRoot + RegistryKeySeperator + subKey).Replace('/', '\\'), writable);
		}

		public string FixupKeyName(RegistryKey key)
		{
			return key.Name.Replace(RegistryHive + RegistryRoot, "").TrimStart('\\').Replace('\\', '/');
		}
	}
}