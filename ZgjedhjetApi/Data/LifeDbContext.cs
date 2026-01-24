using Microsoft.EntityFrameworkCore;
using ZgjedhjetApi.Models.Entities;

namespace ZgjedhjetApi.Data
{
    // YOUR CODE HERE
    public class LifeDbContext : DbContext
    {
        public DbSet<Zgjedhjet> Zgjedhjet { get; set; }

        public LifeDbContext(DbContextOptions<LifeDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Zgjedhjet>(entity =>
            {
                entity.ToTable("Zgjedhjet");

                entity.Property(e => e.Kategoria).HasConversion<string>();
                entity.Property(e => e.Komuna).HasConversion<string>();

                entity.OwnsOne(e => e.Partia);
            });
        }
    }
}
