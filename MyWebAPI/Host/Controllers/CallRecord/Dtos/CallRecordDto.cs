using System.ComponentModel.DataAnnotations;

namespace Host.Controllers.CallRecord
{
    public class CallRecordDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 通话时长，单位s
        /// </summary>
        public int Duration { get; set; }
    }
}