using Microsoft.Win32;

namespace RegistryEditor.Common
{
	public class LocalMachineRegistryHiveProvider : IRegistryHiveProvider
	{
		public RegistryHive GetRegistryHive()
		{
			return RegistryHive.LocalMachine;
		}
	}
}