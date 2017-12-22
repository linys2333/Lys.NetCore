using LysCore.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LysCore.Web
{
    public class ParameterNotNullOrEmptyAttribute : Attribute, IActionFilter
    {
        private readonly string _param;

        public ParameterNotNullOrEmptyAttribute(string param)
        {
            _param = param;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue(_param, out object value);
            Requires.NotNullOrEmpty(value as string, _param);
        }
    }
}
