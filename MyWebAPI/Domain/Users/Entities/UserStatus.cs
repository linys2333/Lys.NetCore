using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus : byte
    {
        [Display(Name = "禁用")]
        Disabled = 0,

        [Display(Name = "启用")]
        Normal = 1
    }
}
