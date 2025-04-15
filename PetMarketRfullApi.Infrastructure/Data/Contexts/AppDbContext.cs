using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMarketRfullApi.Infrastructure.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User, UserRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>(b =>
            {
                b.HasKey(k => new { k.CartId, k.ItemId });
                b.HasOne(p => p.Pet);
                b.HasOne(p => p.Cart)
                    .WithMany(m => m.Items)
                    .HasForeignKey(k => k.CartId);
            });

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

            // cart & cartitem
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.CartId);

            // Настройка связи между Order и User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
