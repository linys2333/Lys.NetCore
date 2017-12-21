using Domain.CallRecord;
using Domain.User;
using LysCore.Controllers;
using LysCore.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Host.Controllers
{
    public class HomeController : LysAuthController
    {
        //private readonly Lazy<UserManager> m_UserManager = new Lazy<UserManager>();

        /// <summary>
        /// 获取首页信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ParameterNotNullOrEmpty("userId")]
        public async Task<object> GetInfo([FromHeader]Guid userId)
        {
            var user = await GetService<UserManager>().GetAsync(userId);
            var callCount = await GetService<CallRecordManager>().CountMyTodayCallsAsync(user.Id);

            return new
            {
                RealName = user.RealName,
                CallCount = callCount
            };
        }
    }
}
