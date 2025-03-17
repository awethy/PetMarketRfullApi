using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;

namespace PetMarketRfullApi.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User, UserRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<User> Users { get; set; }  
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений и category
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.CategoryId);

            // Настройка связи между Product и Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Настройка связи между Order и User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); ;

            // Настройка связи между Order и Product
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict); ;

            // Настройка связи между Order и Pet
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Pet)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PetId)
                .OnDelete(DeleteBehavior.Restrict); ;

            base.OnModelCreating(modelBuilder);
        }
    }
}
