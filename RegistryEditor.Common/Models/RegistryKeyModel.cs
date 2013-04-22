namespace RegistryEditor.Common.Models
{
	public class RegistryKeyModel
	{
		public string KeyName { get; set; }
		public string[] SubKeys { get; set; }
		public RegistryKeyValueModel[] Values { get; set; }
	}
}