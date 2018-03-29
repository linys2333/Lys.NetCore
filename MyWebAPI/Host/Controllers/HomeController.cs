using Domain.CallRecords;
using Domain.Users;
using LysCore.Controllers;
using LysCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Host.Controllers
{
    public class HomeController : LysAuthController
    {
        private readonly LazyService<UserManager> m_UserManager = new LazyService<UserManager>();
        private readonly LazyService<CallRecordManager> m_CallRecordManager = new LazyService<CallRecordManager>();

        /// <summary>
        /// 获取首页信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetInfo([FromHeader]Guid userId)
        {
            var user = await m_UserManager.Instance.GetAsync(userId);
            var callCount = await m_CallRecordManager.Instance.CountMyTodayCallsAsync(user.Id);

            return new
            {
                RealName = user.RealName,
                CallCount = callCount
            };
        }
    }
}
