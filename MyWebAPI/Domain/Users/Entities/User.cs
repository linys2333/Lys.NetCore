using LysCore.Common;
using LysCore.Entities;
using LysCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User : LysEntityBase
    {
        [Required, StringLength(LysConstants.Validations.MoblieStringLength)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string RealName { get; set; }
        
        [Required, StringLength(LysConstants.Validations.PasswordHashStringLength)]
        public string PasswordHash { get; set; }
        
        [Required]
        public Guid CompanyId { get; set; }

        public UserStatus Status { get; set; }
    }

    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByNameAsync(string userName);
    }
}
