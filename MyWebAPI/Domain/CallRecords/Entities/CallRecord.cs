using LysCore.Entities;
using LysCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Domain.CallRecords
{
    public class CallRecord : LysEntityBase
    {
        [Required]
        public Guid OwnerId { get; set; }
        
        public int Duration { get; set; }
        
        public Guid? FileId { get; set; }
    }

    public interface ICallRecordRepository : IRepository<CallRecord>
    {
        Task<int> CountMyTodayCallsAsync(Guid userId);
    }
}
