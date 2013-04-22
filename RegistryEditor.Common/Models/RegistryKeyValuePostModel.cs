using Microsoft.Win32;

namespace RegistryEditor.Common.Models
{
	public class RegistryKeyValuePostModel
	{
		public string ServerName { get; set; }
		public string KeyName { get; set; }
		public string ValueName { get; set; }
		public object Value { get; set; }
		public RegistryValueKind ValueKind { get; set; }
	}
}