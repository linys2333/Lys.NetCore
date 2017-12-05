namespace MyService.Common
{
    public class InvokedResult
    {
        protected InvokedResult()
        {
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public static readonly InvokedResult Success = new InvokedResult { IsSuccess = true };

        public static InvokedResult Fail(string msg) => new InvokedResult { IsSuccess = false, Message = msg };
    }
}