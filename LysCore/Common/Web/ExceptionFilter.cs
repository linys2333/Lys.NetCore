using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Net;

namespace LysCore.Common.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var httpStatusCode = context.Exception is ArgumentNullException
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.InternalServerError;

            var response = AjaxResponse.Fail(GetErrorMessage(context.Exception));

            context.Result = new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            if (httpStatusCode != HttpStatusCode.BadRequest)
            {
                Log.Logger.Error(context.Exception, response.Message);
            }
        }

        private string GetErrorMessage(Exception exception)
        {
            return exception.InnerException == null ? exception.Message : GetErrorMessage(exception.InnerException);
        }
    }
}
