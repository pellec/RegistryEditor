using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace RegistryEditor.Api
{
	public class ValidateModelAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
		{
			if(!actionContext.ModelState.IsValid)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
				                                                                   actionContext.ModelState);
			}
		}
	}
}