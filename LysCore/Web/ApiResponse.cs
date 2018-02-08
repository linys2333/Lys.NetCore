using LysCore.Common;
using LysCore.Exceptions;

namespace LysCore.Web
{
    /// <summary>
    /// API响应数据
    /// </summary>
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public ResponseError Error { get; set; }

        private string m_DefaultCode = LysConstants.Errors.InternalServerError;

        protected ApiResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        protected ApiResponse(string errorMessage)
        {
            IsSuccess = false;
            Error = new ResponseError
            {
                Code = m_DefaultCode,
                Message = errorMessage
            };
        }

        protected ApiResponse(ResponseError error)
        {
            IsSuccess = false;
            Error = error ?? new ResponseError
            {
                Code = m_DefaultCode
            };
        }

        protected ApiResponse(BusinessException exception)
        {
            IsSuccess = false;
            Error = new ResponseError
            {
                Code = exception.ErrorCode,
                Message = exception.Message
            };
        }

        public static ApiResponse Ok() => new ApiResponse(true);

        public static ApiResponse Ok<T>(T data) => new ApiResponse<T>(true, data);

        public static ApiResponse Fail() => new ApiResponse(false);

        public static ApiResponse Fail(string errorMessage) => new ApiResponse(errorMessage);

        public static ApiResponse Fail(ResponseError error) => new ApiResponse(error);

        public static ApiResponse Fail(BusinessException exception) => new ApiResponse(exception);
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse(bool isSuccess, T data) : base(isSuccess)
        {
            Data = data;
        }
    }
}
