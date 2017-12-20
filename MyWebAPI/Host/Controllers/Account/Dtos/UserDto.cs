using System.ComponentModel.DataAnnotations;

namespace Host.Controllers.Account
{
    public class UserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}