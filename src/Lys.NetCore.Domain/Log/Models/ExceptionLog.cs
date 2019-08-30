namespace Lys.NetCore.Domain.Log.Models
{
    public class ExceptionLog : LogBase
    {
        public string ErrorCode { get; set; }

        public string ErrorData { get; set; }
    }
}
