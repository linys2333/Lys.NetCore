namespace Lys.NetCore.Domain.Log.Models
{
    public class LogBase
    {
        public string ProjectCode { get; set; }

        public string Version { get; set; }

        public string OSName { get; set; }

        public string OSVersion { get; set; }

        public string ClientIP { get; set; }

        public string LocalIP { get; set; }
        
        public string SessionId { get; set; }
        
        public string Time { get; set; }
    }
}
