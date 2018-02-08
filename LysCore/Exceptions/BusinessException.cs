using System;

namespace LysCore.Exceptions
{
    /// <summary>
    /// 业务通用异常
    /// </summary>
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string msg, string errorCode = null)
            : base(msg)
        {
            ErrorCode = errorCode ?? nameof(BusinessException);
        }
    }

    /// <summary>
    /// 业务校验异常
    /// </summary>
    public class ValidateException : BusinessException
    {
        public ValidateException(string msg, string errorCode = null)
            : base(msg, errorCode ?? nameof(ValidateException))
        {
        }
    }
}
