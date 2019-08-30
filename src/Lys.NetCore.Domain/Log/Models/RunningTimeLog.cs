namespace Lys.NetCore.Domain.Log.Models
{
    public class RunningTimeLog : LogBase
    {
        public string LogCode { get; set; }

        public string LogData { get; set; }
        
        public int Spend { get; set; }
    }
}
