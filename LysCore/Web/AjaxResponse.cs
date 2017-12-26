using LysCore.Common;

namespace LysCore.Web
{
    public class AjaxResponse
    {
        public bool IsSuccess { get; set; }

        public ResponseError Error { get; set; }

        public AjaxResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public AjaxResponse(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            Error = new ResponseError
            {
                Code = LysConstants.Errors.InternalServerError,
                Message = errorMessage
            };
        }

        public AjaxResponse(bool isSuccess, ResponseError error)
        {
            IsSuccess = isSuccess;
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

        public AjaxResponse(bool isSuccess, T data) : base(isSuccess)
        {
            Data = data;
        }
    }
}
