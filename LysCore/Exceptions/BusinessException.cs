using System;

namespace LysCore.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string msg) : base(msg) { }

        public BusinessException(string errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }
    }

    public class ValidateException : BusinessException
    {
        public ValidateException(string msg) : base(msg) { }

        public ValidateException(string errorCode, string msg) : base(errorCode, msg) { }
    }
}
