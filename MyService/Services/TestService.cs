using Microsoft.EntityFrameworkCore;
using MyService.Common;
using MyService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyService.Services
{
    public class TestService : BaseService
    {
        public async Task<IList<Communication>> GetListAsync(string userId)
        {
            Requires.NotNullOrEmpty(userId, nameof(userId));

            return await DbContext.Set<Communication>().Where(c => c.OwnerId == userId).ToListAsync();
        }
        
        public async Task<InvokedResult> CreateAsync()
        {
            var inputPath = "1.amr";
            var outputPath = "1.mp3";

            var result = FFmpegExt.Convert(inputPath, outputPath);

            return InvokedResult.Success;
        }
    }
}
