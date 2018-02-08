namespace LysCore.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class InvokeResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        
        public static InvokeResult Ok() => new InvokeResult { IsSuccess = true };

        public static InvokeResult Fail(string msg) => new InvokeResult { IsSuccess = false, Message = msg };

        public static InvokeResult<T> Ok<T>(T data) => new InvokeResult<T> { IsSuccess = true, Data = data };

        public static InvokeResult<T> Fail<T>(string msg, T data) => new InvokeResult<T> { IsSuccess = false, Message = msg, Data = data };
    }

    public class InvokeResult<T> : InvokeResult
    {
        public T Data { get; set; }
    }
}