using LysCore.Common;
using LysCore.Common.Extensions;
using LysCore.Exceptions;
using LysCore.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;

namespace LysCore.Web
{
    public class ExceptionFilter : IExceptionFilter
    {
        public virtual void OnException(ExceptionContext context)
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
        }

        private JsonResult InternalErrorHandle(Exception exception)
        {
            var response = ApiResponse.Fail(exception.Message);

            LysLog.Logger.Error(JsonConvert.SerializeObject(response.Error));

            return new JsonResult(response.Error)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        private JsonResult BadRequestHandle(Exception exception)
        {
            var response = ApiResponse.Fail(new ResponseError
            {
                Code = LysConstants.Errors.BadRequest,
                Message = exception.Message
            });

            LysLog.Logger.Error(JsonConvert.SerializeObject(response.Error));

            return new JsonResult(response.Error)
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
        }

        private JsonResult BusinessErrorHandle(BusinessException exception)
        {
            var response = ApiResponse.Fail(exception);

            LysLog.BizLogger.Error(JsonConvert.SerializeObject(response));

            return new JsonResult(response)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
