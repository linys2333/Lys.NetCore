using Microsoft.EntityFrameworkCore;
using MyWebAPI.Stores.Entity;

namespace MyWebAPI.Managers
{
    public class BazaService
    {
        protected BazaService()
        {
        }

        public BazaService(BazaDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BazaDbContext DbContext { get; set; }
    }
}