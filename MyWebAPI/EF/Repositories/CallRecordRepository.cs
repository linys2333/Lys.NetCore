using Domain.CallRecord;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Repositories
{
    public class CallRecordRepository : MyRepository<CallRecord>, ICallRecordRepository
    {
        public CallRecordRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<int> CountMyTodayCallsAsync(Guid userId)
        {
            var today = DateTime.Now.Date;
            var count = await m_DbContext.CallRecords.Where(c => c.OwnerId == userId && c.Created.Date == today).CountAsync();
            return count;
        }
    }
}
