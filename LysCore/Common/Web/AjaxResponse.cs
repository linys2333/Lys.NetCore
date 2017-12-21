namespace LysCore.Common.Web
{
    public class AjaxResponse
    {
        public bool Succeeded { get; set; }

        public ResponseError Error { get; set; }

        public AjaxResponse(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public AjaxResponse(bool succeeded, string errorMessage)
        {
            Succeeded = succeeded;
            Error = new ResponseError
            {
                Code = LysConstants.Errors.InternalServerError,
                Message = errorMessage
            };
        }

        public AjaxResponse(bool succeeded, ResponseError error)
        {
            Succeeded = succeeded;
            Error = error ?? new ResponseError
            {
                Code = LysConstants.Errors.InternalServerError,
            };
        }

        public static AjaxResponse Ok() => new AjaxResponse(true);

        public static AjaxResponse Ok<T>(T data) => new AjaxResponse<T>(true, data);

        public static AjaxResponse Fail(string errorMessage) => new AjaxResponse(false, errorMessage);

        public static AjaxResponse Fail(ResponseError error) => new AjaxResponse(false, error);
    }

    public class AjaxResponse<T> : AjaxResponse
    {
        public T Data { get; set; }

        public AjaxResponse(bool succeeded, T data) : base(succeeded)
        {
            Data = data;
        }
    }
}
