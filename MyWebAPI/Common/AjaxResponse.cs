namespace MyWebAPI.Common
{
    public class AjaxResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public AjaxResponse()
        {
        }

        public AjaxResponse(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static AjaxResponse Ok(string message = null)
        {
            return new AjaxResponse(true, message);
        }

        public static AjaxResponse Fail(string message = null)
        {
            return new AjaxResponse(false, message);
        }
    }

    public class AjaxResponse<T> : AjaxResponse
    {
        public T Data { get; set; }

        public AjaxResponse(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
    }
}
