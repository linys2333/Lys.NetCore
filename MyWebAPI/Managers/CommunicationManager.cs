using Microsoft.EntityFrameworkCore;
using MyWebAPI.Common;
using MyWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPI.Managers
{
    public class CommunicationManager : BazaService
    {
        public async Task<IList<Communication>> GetListAsync(string userId)
        {
            Requires.NotNullOrEmpty(userId, nameof(userId));

            return await DbContext.Set<Communication>().Where(c => c.OwnerId == userId).ToListAsync();
        }
        
        public async Task<InvokedResult> CreateAsync(Communication communication)
        {
            Requires.NotNull(communication, nameof(communication));

            var unRemovedCommunication = await DbContext.Set<Communication>().FirstOrDefaultAsync(c =>
                    c.OwnerId == communication.OwnerId &&
                    c.JobId == communication.JobId &&
                    c.CandidateId == communication.CandidateId &&
                    c.Status != CommunicationStatus.Removed);

            if (unRemovedCommunication == null)
            {
                communication.Status = CommunicationStatus.Unprocessed;

                DbContext.Set<Communication>().Add(communication);
                await DbContext.SaveChangesAsync();
            }

            return InvokedResult.Success;
        }
    }
}
