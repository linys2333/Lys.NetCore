namespace LysCore.Common
{
    public class InvokedResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public static InvokedResult Ok() => new InvokedResult { IsSuccess = true };

        public static InvokedResult Fail(string msg) => new InvokedResult { IsSuccess = false, Message = msg };

        public static InvokedResult<T> Ok<T>(T data) => new InvokedResult<T> { IsSuccess = true, Data = data };

        public static InvokedResult<T> Fail<T>(string msg, T data) => new InvokedResult<T> { IsSuccess = false, Message = msg, Data = data };
    }

    public class InvokedResult<T> : InvokedResult
    {
        public T Data { get; set; }
    }
}