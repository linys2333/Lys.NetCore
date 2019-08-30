using Lys.NetCore.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Lys.NetCore.Portal.DTOs
{
    /// <summary>
    /// 通话记录
    /// </summary>
    public class CallRecordRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = MyConstants.Errors.RequiredErrorMessage)]
        [StringLength(MyConstants.Validations.MobileStringLength, ErrorMessage = MyConstants.Errors.StringLengthErrorMessage)]
        public string Mobile { get; set; }

        /// <summary>
        /// 通话时长，单位s
        /// </summary>
        public int Duration { get; set; }
    }
}