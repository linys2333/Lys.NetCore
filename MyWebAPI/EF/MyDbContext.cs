using Domain.CallRecords;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EF
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options: options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallRecord>().ToTable("TestCallRecords");

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<CallRecord> CallRecords { get; set; }
    }
}
