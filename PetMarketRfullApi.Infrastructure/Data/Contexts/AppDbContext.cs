using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Models.Products;

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
        public DbSet<OtherItem> OtherItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>(b =>
            {
                b.HasKey(k => new { k.CartId, k.Id });
                b.HasOne(p => p.Product);
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
            modelBuilder.Entity<OtherItem>()
                .HasOne(p => p.Category)
                .WithMany(c => c.OtherItems)
                .HasForeignKey(p => p.CategoryId);

            //// cart & cartitem
            //modelBuilder.Entity<Cart>(b =>
            //{
            //    b.HasMany(c => c.Items)
            //        .WithOne(ci => ci.Cart)
            //        .HasForeignKey(ci => ci.CartId);
            //});

            modelBuilder.Entity<Order>(b =>
            {
                b.HasOne(o => o.Cart)
                    .WithOne(c => c.Order)
                    .HasForeignKey<Order>(o => o.CartId);
                b.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });               

            base.OnModelCreating(modelBuilder);
        }
    }
}
