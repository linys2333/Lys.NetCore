using System.ComponentModel.DataAnnotations;

namespace Host.Controllers.Account
{
    public class ClientDto
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        [Required]
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [Required]
        public string Secret { get; set; }
    }
}