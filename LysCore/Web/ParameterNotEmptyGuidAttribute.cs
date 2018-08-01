using LysCore.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LysCore.Web
{
    /// <summary>
    /// Guid请求参数空校验
    /// </summary>
    public class ParameterNotEmptyGuidAttribute : Attribute, IActionFilter
    {
        private readonly string m_Param;

        public ParameterNotEmptyGuidAttribute(string param)
        {
            m_Param = param;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue(m_Param, out var value);
            Requires.NotNullOrEmpty((Guid?)value, m_Param);
        }
    }
}
