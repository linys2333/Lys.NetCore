using System;

namespace LysCore.Common.Extensions
{
    public static class ExceptionExt
    {
        /// <summary>
        /// 递归获取最内层异常
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
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
