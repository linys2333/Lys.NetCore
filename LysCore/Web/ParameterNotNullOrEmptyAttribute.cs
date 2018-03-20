using LysCore.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LysCore.Web
{
    /// <summary>
    /// 字符串请求参数空校验
    /// </summary>
    public class ParameterNotNullOrEmptyAttribute : Attribute, IActionFilter
    {
        private readonly string m_Param;

        public ParameterNotNullOrEmptyAttribute(string param)
        {
            m_Param = param;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue(m_Param, out var value);
            Requires.NotNullOrEmpty(value as string, m_Param);
        }
    }
}
