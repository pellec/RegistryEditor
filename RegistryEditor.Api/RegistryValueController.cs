using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using RegistryEditor.Common;
using RegistryEditor.Common.Models;
using RegistryEditor.Common.NetworkShare;

namespace RegistryEditor.Api
{
	public class RegistryValueController : ApiController
	{
		private readonly IRegistryProvider _registryProvider;
		private readonly INetworkShare _networkShare;

		public RegistryValueController(IRegistryProvider registryProvider, INetworkShare networkShare)
		{
			_registryProvider = registryProvider;
			_networkShare = networkShare;
		}

		public HttpResponseMessage Post(RegistryKeyValuePostModel model)
		{
			if (!_networkShare.TryConnect(model.ServerName))
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
											string.Format("Could not connect to: '{0}'", model.ServerName));
			}

			using (var key = _registryProvider.GetRegistryAndOpenSubKey(model.ServerName, model.KeyName, true))
			{
				if (key == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
					                                   string.Format("The key '{0}' could not be found on server '{1}'.", model.KeyName, model.ServerName));
				}

				object value;
				if (!TryGetValue(model, out value))
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
					                                   string.Format("The value of the key: '{0}' is invalid.", model.KeyName));
				}

				key.SetValue(model.ValueName, value, model.ValueKind);
				return Request.CreateResponse(HttpStatusCode.Created);
			}
		}

		private static bool TryGetValue(RegistryKeyValuePostModel keyValue, out object value)
		{
			switch (keyValue.ValueKind)
			{
				case RegistryValueKind.String:
					{
						value = keyValue.Value;
						return true;
					}
				case RegistryValueKind.MultiString:
					{
						value = ((List<string>)keyValue.Value).ToArray();
						return true;
					}
			}

			value = null;
			return false;
		}
	}
}