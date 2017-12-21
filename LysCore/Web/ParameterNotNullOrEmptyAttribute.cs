using LysCore.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LysCore.Web
{
    public class ParameterNotNullOrEmptyAttribute : ActionFilterAttribute
    {
        private readonly string _param;

        public ParameterNotNullOrEmptyAttribute(string param)
        {
            _param = param;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue(_param, out object value);
            Requires.NotNullOrEmpty(value as string, _param);
        }
    }
}
