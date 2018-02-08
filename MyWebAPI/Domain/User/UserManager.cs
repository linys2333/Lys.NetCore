using LysCore.Common;
using LysCore.Exceptions;
using LysCore.Services;
using System;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Domain.User
{
    public class UserManager : LysDomain
    {
        private readonly LazyService<IUserRepository> m_UserRepository = new LazyService<IUserRepository>();
        
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetAsync(Guid userId)
        {
            Requires.NotNullGuid(userId, nameof(userId));
            var user = await m_UserRepository.Instance.GetAsync(userId);
            return user;
        }

        /// <summary>
        /// 密码模式验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Guid> PasswordSignInAsync(string userName, string password)
        {
            Requires.NotNullOrEmpty(userName, nameof(userName));
            Requires.NotNullOrEmpty(password, nameof(password));
            
            var user = await m_UserRepository.Instance.FindByNameAsync(userName);
            var isChecked = CheckSignInAsync(user, password);

            if (!isChecked)
            {
                throw new ValidateException("用户名或者密码错误");
            }

            return user.Id;
        }

        /// <summary>
        /// 登陆校验
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool CheckSignInAsync(User user, string password)
        {
            if (user == null || !Crypto.VerifyHashedPassword(user.PasswordHash, password))
            {
                return false;
            }

            switch (user.Status)
            {
                case UserStatus.Disabled:
                    return false;
                case UserStatus.Normal:
                    break;
                default:
                    throw new ValidateException("不受支持的用户状态");
            }

            return true;
        }
    }
}
