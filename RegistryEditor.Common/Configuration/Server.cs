using System.Configuration;

namespace RegistryEditor.Common.Configuration
{
	public class Server : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
			set
			{
				this["name"] = value;
			}
		}	
	
		[ConfigurationProperty("share", IsRequired = true)]
		public string Share
		{
			get
			{
				return (string)this["share"];
			}
			set
			{
				this["share"] = value;
			}
		}

		[ConfigurationProperty("user", IsRequired = true)]
		public string User
		{
			get
			{
				return (string)this["user"];
			}
			set
			{
				this["user"] = value;
			}
		}

		[ConfigurationProperty("password", IsRequired = true)]
		public string Password
		{
			get
			{
				return (string)this["password"];
			}
			set
			{
				this["password"] = value;
			}
		}	
	}

	[ConfigurationCollection(typeof(Server), AddItemName = "server")]
	public class ServerCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new Server();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((Server)element).Name;
		}
	}

	public class AppSection : ConfigurationSection
	{
		[ConfigurationProperty("servers", IsDefaultCollection = true, IsRequired = true)]
		public ServerCollection Servers
		{
			get { return this["servers"] as ServerCollection; }
		}		
	}
}