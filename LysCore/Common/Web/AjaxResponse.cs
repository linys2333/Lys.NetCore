namespace LysCore.Common.Web
{
    public class AjaxResponse
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public AjaxResponse(bool succeeded, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static AjaxResponse Ok() => new AjaxResponse(true);

        public static AjaxResponse Ok<T>(T data) => new AjaxResponse<T>(true, data);

        public static AjaxResponse Fail(string message) => new AjaxResponse(false, message);
    }

    public class AjaxResponse<T> : AjaxResponse
    {
        public T Data { get; set; }

        public AjaxResponse(bool succeeded, T data, string message = null) : base(succeeded, message)
        {
            Data = data;
        }
    }
}
