using System;
using System.Collections.Generic;

namespace Lys.NetCore.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorMessage(this Exception exception)
        {
            var baseException = exception.GetBaseException();

            if (baseException is AggregateException aggException)
            {
                var errMsgs = new List<string>();
                var index = 0;
                foreach (var ex in aggException.InnerExceptions)
                {
                    errMsgs.Add($"(#{index++}){ex.GetBaseException().Message}");
                }

                return string.Join("\r\n", errMsgs);
            }

            return baseException.Message;
        }
    }
}
