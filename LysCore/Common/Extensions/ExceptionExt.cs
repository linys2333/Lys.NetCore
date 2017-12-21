using System;

namespace LysCore.Common.Extensions
{
    public static class ExceptionExt
    {
        public static Exception GetException(this Exception exception)
        {
            return exception.InnerException == null ? exception : GetException(exception.InnerException);
        }

        public static string GetMessage(this Exception exception)
        {
            return exception.GetException().Message;
        }
    }
}
