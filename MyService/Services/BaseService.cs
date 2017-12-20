using MyService.Data;

namespace MyService.Services
{
    public class BaseService
    {
        protected BaseService()
        {
        }

        public BaseService(MyDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public MyDbContext DbContext { get; set; }
    }
}