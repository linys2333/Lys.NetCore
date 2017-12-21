using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace LysCore.Web
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            AjaxResponse response = null;

            if (context.Result is EmptyResult)
            {
                response = AjaxResponse.Ok();
            }
            else if (context.Result is ObjectResult)
            {
                response = AjaxResponse.Ok(((ObjectResult)context.Result).Value);
            }
            else if (context.Result is ContentResult)
            {
                response = AjaxResponse.Ok(((ContentResult) context.Result).Content);
            }

            if (response != null)
            {
                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }
    }
}
