using Lys.NetCore.Domain.CallRecords.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lys.NetCore.EF.Repositories
{
    public class CallRecordRepository : MyRepository<CallRecord>, ICallRecordRepository
    {
        public CallRecordRepository(MyDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 统计今日通话次数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> CountMyTodayCallsAsync(Guid userId)
        {
            var today = DateTime.Now.Date;
            var count = await m_DbContext.Set<CallRecord>().Where(c => c.OwnerId == userId && c.Created.Date == today).CountAsync();
            return count;
        }
    }
}
