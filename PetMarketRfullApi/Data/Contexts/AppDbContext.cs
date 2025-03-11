using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;

namespace PetMarketRfullApi.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
