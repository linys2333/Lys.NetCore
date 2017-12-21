using System;

namespace LysCore.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string msg, string errorCode = null)
            : base(msg)
        {
            ErrorCode = errorCode ?? nameof(BusinessException);
        }
    }

    public class ValidateException : BusinessException
    {
        public ValidateException(string msg, string errorCode = null)
            : base(msg, errorCode ?? nameof(ValidateException))
        {
        }
    }
}
