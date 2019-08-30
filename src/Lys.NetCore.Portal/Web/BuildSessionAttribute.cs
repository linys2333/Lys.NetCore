using Lys.NetCore.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Lys.NetCore.Portal.Web
{
    public class BuildSessionAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            var session = new MySession
            {
                ClientIP = httpContext.Connection.RemoteIpAddress.ToString(),
                LocalIP = httpContext.Connection.LocalIpAddress.ToString(),
                SessionId = Guid.NewGuid().ToString()
            };

            httpContext.Items.Add(nameof(MySession), session);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}