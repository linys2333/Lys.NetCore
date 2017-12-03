using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;
using System.Collections.Generic;

namespace MyWebAPI.Stores.Entity
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

        public ISet<Communication> Communications { get; set; }
    }
}
