using LysCore.Common.Extensions;
using LysCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;
using LysCore.Common;

namespace LysCore.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception.GetException();
            var response = AjaxResponse.Fail(exception.Message);

            if (exception is ArgumentNullException)
            {
                response.Error.Code = LysConstants.Errors.BadRequest;
            }
            else if (exception is BusinessException)
            {
                response.Error.Code = (exception as BusinessException).ErrorCode;
            }

            context.Result = new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            Log.Logger.Error(context.Exception, JsonConvert.SerializeObject(response.Error));
        }
    }
}
