using System.ComponentModel.DataAnnotations;

namespace Host.Controllers.CallRecord
{
    /// <summary>
    /// 通话记录
    /// </summary>
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