using Microsoft.Win32;

namespace RegistryEditor.Common.Models
{
	public class RegistryKeyValueModel
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public RegistryValueKind ValueKind { get; set; }
		public string KeyName { get; set; }
	}
}