using Microsoft.EntityFrameworkCore;
using MyService.Models;

namespace MyService.Stores.Entity
{
    public class BazaDbContext : DbContext
    {
        public BazaDbContext(DbContextOptions<BazaDbContext> options)
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
