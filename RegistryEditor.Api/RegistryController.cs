using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Win32;
using RegistryEditor.Common;
using RegistryEditor.Common.Models;
using RegistryEditor.Common.NetworkShare;

namespace RegistryEditor.Api
{
	public class RegistryController : ApiController
	{
		private readonly IRegistryProvider _registryProvider;
		private readonly INetworkShare _networkShare;

		public RegistryController(IRegistryProvider registryProvider, INetworkShare networkShare)
		{
			_registryProvider = registryProvider;
			_networkShare = networkShare;
		}

		public HttpResponseMessage Get(string serverName, string subKey = "")
		{
			HttpResponseMessage errorResponse;
			if (!TryConnectToShare(serverName, out errorResponse))
			{
				return errorResponse;
			}

			using (var key = _registryProvider.GetRegistryAndOpenSubKey(serverName, subKey, true))
			{
				if (key == null)
				{
					return Request.CreateResponse(HttpStatusCode.NotFound);
				}

				var r = new RegistryKeyModel
					{
						KeyName = _registryProvider.FixupKeyName(key),
						SubKeys = key.GetSubKeyNames().OrderBy(k => k.ToString()).ToArray(),
						Values = BuildKeyValues(key).OrderBy(v => v.Name).ToArray()
					};

				return Request.CreateResponse(HttpStatusCode.OK, r);
			}
		}

		[ValidateModel]
		public HttpResponseMessage Post(RegistryKeyPostModel model)
		{
			HttpResponseMessage errorResponse;
			if (!TryConnectToShare(model.ServerName, out errorResponse))
			{
				return errorResponse;
			}

			using (var key = _registryProvider.GetRegistryAndOpenSubKey(model.ServerName, model.SubKey, true))
			{
				if (key == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
					                                   string.Format("Sub key not present at: '{0}'", model.ServerName));
				}

				var newKey = key.CreateSubKey(model.NewKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
				if (newKey == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
					                                   string.Format("New key '{0}' could not be created.", model.NewKey));
				}

				return Request.CreateResponse(HttpStatusCode.Created);
			}
		}

		public HttpResponseMessage Delete(string serverName, string key)
		{
			return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "This is an extremly dangerous operation!");
//			HttpResponseMessage errorResponse;
//			if (!TryConnectToShare(serverName, out errorResponse))
//			{
//				return errorResponse;
//			}
//
//			using (var subKey = _registryProvider.GetRegistryAndOpenSubKey(serverName, key, true))
//			{
//				if (subKey == null)
//				{
//					return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
//					                                   string.Format("Sub key not present at: '{0}'", serverName));
//				}
//			}
//
//			return Request.CreateResponse(HttpStatusCode.NoContent);
		}

		private bool TryConnectToShare(string serverName, out HttpResponseMessage errorResponse)
		{
			if (!_networkShare.TryConnect(serverName))
			{
				errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
															string.Format("Could not connect to: '{0}'", serverName));
				return false;
			}
			errorResponse = null;
			return true;
		}

		private IEnumerable<RegistryKeyValueModel> BuildKeyValues(RegistryKey key)
		{
			return key.GetValueNames().Select(valueName => new RegistryKeyValueModel
			{
				KeyName = _registryProvider.FixupKeyName(key),
				Name = valueName,
				Value = key.GetValue(valueName),
				ValueKind = key.GetValueKind(valueName)
			});
		}
	}
}