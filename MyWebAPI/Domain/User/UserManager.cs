﻿using LysCore.Common;
using LysCore.Domain;
using LysCore.Exceptions;
using System;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Domain.User
{
    public class UserManager : LysDomain
    {
        private readonly IUserRepository m_UserRepository;

        public UserManager(IUserRepository userRepository)
        {
            m_UserRepository = userRepository;
        }

        public async Task<User> GetAsync(Guid userId)
        {
            Requires.NotNullGuid(userId, nameof(userId));
            var user = await m_UserRepository.GetAsync(userId);
            return user;
        }

        public async Task<Guid> PasswordSignInAsync(string userName, string password)
        {
            Requires.NotNullOrEmpty(userName, nameof(userName));
            Requires.NotNullOrEmpty(password, nameof(password));
            
            var user = await m_UserRepository.FindByNameAsync(userName);
            var result = CheckSignInAsync(user, password);

            if (!result)
            {
                var errorMessage = "用户名或者密码错误";
                throw new ValidateException(errorMessage);
            }

            return user.Id;
        }

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
                    throw new NotSupportedException("不受支持的用户状态");
            }

            return true;
        }
    }
}