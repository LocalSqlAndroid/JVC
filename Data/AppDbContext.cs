using Microsoft.EntityFrameworkCore;
using SqlServerApi.Models;
using System.Collections.Generic;
namespace SqlServerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<OverallReport> DyOverall { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OverallReport>().HasNoKey();

            // other entities configurations...
        }
    }
}
