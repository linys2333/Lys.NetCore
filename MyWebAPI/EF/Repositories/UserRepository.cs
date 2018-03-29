using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EF.Repositories
{
    public class UserRepository : MyRepository<User>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 根据姓名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<User> FindByNameAsync(string userName)
        {
            var user = await m_DbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }
    }
}
