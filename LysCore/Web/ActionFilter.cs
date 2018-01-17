using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace LysCore.Web
{
    public class ActionFilter : IActionFilter
    {
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            ApiResponse response = null;

            if (context.Result is EmptyResult)
            {
                response = ApiResponse.Ok();
            }
            else if (context.Result is ObjectResult)
            {
                response = ApiResponse.Ok(((ObjectResult)context.Result).Value);
            }
            else if (context.Result is ContentResult)
            {
                response = ApiResponse.Ok(((ContentResult) context.Result).Content);
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
