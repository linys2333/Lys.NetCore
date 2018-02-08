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
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 统一封装异常响应格式
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnException(ExceptionContext context)
        {
            var exception = context.Exception.GetException();

            switch (exception)
            {
                case ArgumentException _:
                    context.Result = BadRequestHandle(exception);
                    break;
                case BusinessException _:
                    context.Result = BusinessErrorHandle(exception as BusinessException);
                    break;
                default:
                    context.Result = InternalErrorHandle(exception);
                    break;
            }
        }

        /// <summary>
        /// 500响应处理
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private JsonResult InternalErrorHandle(Exception exception)
        {
            var response = ApiResponse.Fail(exception.Message);

            LysLog.Logger.Error(JsonConvert.SerializeObject(response.Error));

            return new JsonResult(response.Error)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        /// <summary>
        /// 400响应处理
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 普通异常处理
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
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
