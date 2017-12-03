using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public enum CommunicationStatus : short
    {
        [Display(Name = "未处理")]
        Unprocessed = 0,

        [Display(Name = "已移除")]
        Removed = 1,

        [Display(Name = "已推荐")]
        Recommended = 2
    }
}
