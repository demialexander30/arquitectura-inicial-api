using Microsoft.EntityFrameworkCore;
using SalesOnline.Domain.Entities;

namespace SalesOnline.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FlightNumber).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Origin).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Destination).IsRequired().HasMaxLength(50);
            });
        }
    }
}
