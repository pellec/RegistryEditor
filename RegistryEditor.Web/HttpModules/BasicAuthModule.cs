using System;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace RegistryEditor.Web.HttpModules
{
	public class BasicAuthModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += Authorize;
		}

		private static void Authorize(object sender, EventArgs args)
		{
			var app = (HttpApplication)sender;

			string auth = app.Request.Headers["Authorization"];
			if (auth != null)
			{
				var parts = auth.Split(' ');
				if (parts.Length == 2 && parts[0] == "Basic")
				{
					var usernamePassword = Encoding.Default.GetString(Convert.FromBase64String(parts[1])).Split(':');
					if (usernamePassword.Length == 2 && usernamePassword[0] == "Hello" && usernamePassword[1] == "World")
					{
						app.Context.User = new GenericPrincipal(new GenericIdentity(usernamePassword[0], "RegistryEditor"),
						                                        new string[] {});
						return;
					}
				}
			}

			app.Context.Response.StatusCode = 401;
			app.Context.Response.AppendHeader("WWW-Authenticate", "Basic realm=\"RegistryEditor\"");
			app.Context.Response.Write("Not authorized");
		}

		public void Dispose()
		{
		}
	}
}