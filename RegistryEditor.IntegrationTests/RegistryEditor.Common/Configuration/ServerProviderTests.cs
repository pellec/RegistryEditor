using NUnit.Framework;
using RegistryEditor.Common.Configuration;

namespace RegistryEditor.IntegrationTests.RegistryEditor.Common.Configuration
{
	[TestFixture]
	public class ServerProviderTests
	{
		[Test]
		public void Should_return_a_server()
		{
			var sp = new ServerProvider();
			Server server;
			var result = sp.TryGetServer("site1", out server);

			Assert.That(result, Is.True);
			Assert.That(server.Share, Is.EqualTo("Share"));
		}
	}
}
