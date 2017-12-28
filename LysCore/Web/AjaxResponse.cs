using LysCore.Common;
using LysCore.Exceptions;

namespace LysCore.Web
{
    public class AjaxResponse
    {
        public bool IsSuccess { get; set; }

        public ResponseError Error { get; set; }

        private string m_DefaultCode = LysConstants.Errors.InternalServerError;

        protected AjaxResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        protected AjaxResponse(string errorMessage)
        {
            IsSuccess = false;
            Error = new ResponseError
            {
                Code = m_DefaultCode,
                Message = errorMessage
            };
        }

        protected AjaxResponse(ResponseError error)
        {
            IsSuccess = false;
            Error = error ?? new ResponseError
            {
                Code = m_DefaultCode
            };
        }

        protected AjaxResponse(BusinessException exception)
        {
            IsSuccess = false;
            Error = new ResponseError
            {
                Code = exception.ErrorCode,
                Message = exception.Message
            };
        }

        public static AjaxResponse Ok() => new AjaxResponse(true);

        public static AjaxResponse Ok<T>(T data) => new AjaxResponse<T>(true, data);

        public static AjaxResponse Fail() => new AjaxResponse(false);

        public static AjaxResponse Fail(string errorMessage) => new AjaxResponse(errorMessage);

        public static AjaxResponse Fail(ResponseError error) => new AjaxResponse(error);

        public static AjaxResponse Fail(BusinessException exception) => new AjaxResponse(exception);
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
