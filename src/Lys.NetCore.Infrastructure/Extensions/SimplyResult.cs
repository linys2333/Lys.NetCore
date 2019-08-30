namespace Lys.NetCore.Infrastructure.Extensions
{
    public class SimplyResult
    {
        public bool IsSuccess { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        
        public static SimplyResult Ok() => new SimplyResult { IsSuccess = true };

        public static SimplyResult<T> Ok<T>(T data) => new SimplyResult<T> { IsSuccess = true, Data = data };

        public static SimplyResult Fail(string errorCode, string errorMsg) => new SimplyResult
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMsg
        };
        
        public static SimplyResult<T> Fail<T>(string errorCode, string errorMsg, T data) => new SimplyResult<T>
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            ErrorMessage = errorMsg,
            Data = data
        };
    }

    public class SimplyResult<T> : SimplyResult
    {
        public T Data { get; set; }
    }
}