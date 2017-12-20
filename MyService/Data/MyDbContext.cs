using Microsoft.EntityFrameworkCore;
using MyService.Models;

namespace MyService.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options: options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Communication>().ToTable("Communications");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Communication> Communications { get; set; }
    }
}
