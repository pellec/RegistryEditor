using Microsoft.Win32;

namespace RegistryEditor.Common
{
	public interface IRegistryHiveProvider
	{
		RegistryHive GetRegistryHive();
	}
}