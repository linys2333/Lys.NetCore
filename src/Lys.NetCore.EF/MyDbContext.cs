using Lys.NetCore.Domain.CallRecords.Models;
using Microsoft.EntityFrameworkCore;

namespace Lys.NetCore.EF
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options: options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallRecord>().ToTable("TestCallRecords");

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<CallRecord> CallRecords { get; set; }
    }
}
