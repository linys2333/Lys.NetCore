using LysCore.Common;
using LysCore.Common.Extensions;
using LysCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net;

namespace LysCore.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception.GetException();

            if (exception is ArgumentException)
            {
                context.Result = BadRequestHandle(exception);
            }
            else if (exception is BusinessException)
            {
                context.Result = BusinessErrorHandle(exception as BusinessException);
            }
            else
            {
                context.Result = InternalErrorHandle(exception);
            }

            var response = context.Result as JsonResult;
            Log.Logger.Error(context.Exception, JsonConvert.SerializeObject(response.Value));
        }

        private JsonResult InternalErrorHandle(Exception exception)
        {
            var response = AjaxResponse.Fail(exception.Message);
            return new JsonResult(response.Error)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        private JsonResult BadRequestHandle(Exception exception)
        {
            var response = AjaxResponse.Fail(new ResponseError
            {
                Code = LysConstants.Errors.BadRequest,
                Message = exception.Message
            });
            return new JsonResult(response.Error)
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
        }

        private JsonResult BusinessErrorHandle(BusinessException exception)
        {
            var response = AjaxResponse.Fail(exception);
            return new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
