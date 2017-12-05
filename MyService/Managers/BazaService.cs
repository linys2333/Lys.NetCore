using MyService.Stores.Entity;

namespace MyService.Managers
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