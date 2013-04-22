using System;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using NUnit.Framework;
using RegistryEditor.Common.Models;

namespace RegistryEditor.IntegrationTests.RegistryEditor.Api
{
	[TestFixture]
	public class RegistryApiControllerTests
	{

		[Test]
		public void Should_return_ok_response_from_local_machine_on_get()
		{
			using (var c = new HttpClientFactory().Create())
			{
				var response = c.GetAsync("registry?serverName=YourComputerName").Result;
				var key = new JavaScriptSerializer().Deserialize<RegistryKeyModel>(response.Content.ReadAsStringAsync().Result);

				Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

				foreach (var subKey in key.SubKeys)
				{
					Console.WriteLine(subKey);
				}
			}
		}

		[Test]
		public void Should_create_new_key_on_post()
		{
			var serverName = "YourComputerName";
			var subKey = "";
			var newKey = "justatest";

			using(var c = new HttpClientFactory().Create())
			{
				var model = new
					{
						serverName,
						subKey,
						newKey
					};

				var responseMessage = c.PostAsJsonAsync("Registry", model).Result;

				Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
			}
		}
	}
}