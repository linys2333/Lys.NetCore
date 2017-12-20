using System.ComponentModel.DataAnnotations;

namespace Host.Controllers.Account
{
    public class LoginDto
    {
        [Required]
        public ClientDto Client { get; set; }
        
        [Required]
        public UserDto User { get; set; }
    }
}