using Lys.NetCore.Domain.Log;
using Lys.NetCore.Infrastructure.Extensions;
using Lys.NetCore.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Lys.NetCore.Portal.Web
{
    public class WebApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            if (context.Exception is ArgumentNullException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }

            var response = new WebApiResponse
            {
                Success = false,
                Error = new ErrorDescriber
                {
                    Code = httpStatusCode.ToString(),
                    Description = context.Exception.GetErrorMessage()
                }
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK,
                DeclaredType = typeof(WebApiResponse)
            };

            if (httpStatusCode != HttpStatusCode.BadRequest)
            {
                var logServer = IoCExtensions.GetService<LogService>();
                logServer.LogException(context.Exception, "WebApiException");
            }
        }
    }
}
