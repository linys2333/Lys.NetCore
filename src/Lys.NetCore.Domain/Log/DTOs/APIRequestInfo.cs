namespace Lys.NetCore.Domain.Log.DTOs
{
    public class APIRequestInfo
    {
        public string Method { get; set; }

        public string Url { get; set; }
        
        public object Paras { get; set; }

        public int Spend { get; set; }

        public bool IsSuccess { get; set; }
    }
}
