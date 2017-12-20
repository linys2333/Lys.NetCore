using Domain.Auth;
using Domain.User;
using IdentityServer4.AccessTokenValidation;
using LysCore.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Host.Controllers.Account
{
    public class AccountController : LysController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginDto">用户登录信息</param>
        /// <returns></returns>
        [HttpPost("~/[action]")]
        public async Task<object> Login([FromBody]LoginDto loginDto)
        {
            var userId = await GetService<UserManager>().PasswordSignInAsync(loginDto.User.UserName, loginDto.User.Password);
            var token = await GetService<AuthService>().GetToken(loginDto.Client.ClientId, loginDto.Client.Secret);

            return new
            {
                UserId = userId,
                Token = token
            };
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("~/[action]")]
        public async Task Logout([FromBody]ClientDto clientDto, [FromHeader]string authorization)
        {
            var token = GetToken(authorization);
            await GetService<AuthService>().RevokeToken(clientDto.ClientId, clientDto.Secret, token);
        }

        private string GetToken(string authorizationHeader)
        {
            var token = authorizationHeader?.Replace($"{IdentityServerAuthenticationDefaults.AuthenticationScheme} ", "");
            return token;
        }
    }
}
