using Lys.NetCore.Infrastructure.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Lys.NetCore.Domain.CallRecords.Models
{
    public class CallRecord : EntityBase
    {
        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string Mobile { get; set; }

        public int Duration { get; set; }

        [Required]
        public Guid FileId { get; set; }
    }

    public interface ICallRecordRepository : IRepository<CallRecord>
    {
        Task<int> CountMyTodayCallsAsync(Guid userId);
    }
}
