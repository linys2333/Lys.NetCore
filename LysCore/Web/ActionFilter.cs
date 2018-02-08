using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace LysCore.Web
{
    /// <summary>
    /// Action过滤器
    /// </summary>
    public class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 统一处理请求数据
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {

        }

        /// <summary>
        /// 统一封装响应数据格式
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            ApiResponse response = null;

            switch (context.Result)
            {
                case EmptyResult _:
                    response = ApiResponse.Ok();
                    break;
                case ObjectResult _:
                    response = ApiResponse.Ok(((ObjectResult)context.Result).Value);
                    break;
                case ContentResult _:
                    response = ApiResponse.Ok(((ContentResult) context.Result).Content);
                    break;
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
