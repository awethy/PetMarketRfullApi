using Microsoft.EntityFrameworkCore.Query;
using PetMarketRfullApi.Controllers;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Sevices;

namespace PetMarketRfullApi.Data.Repositories
{
    public class UnitOfWork : BaseRepositories, IUnitOfWork
    {
        public UnitOfWork(AppDbContext appDbContext) : base(appDbContext) 
        {
            Categories = new CategoryRepository(_appDbContext);
            Pets = new PetRepository(_appDbContext);
            Users = new UserRepository(_appDbContext);
            Orders = new OrderRepository(_appDbContext); 
            Carts = new CartRepository(_appDbContext);
            CartItems = new CartItemRepository(_appDbContext);
        }

        public ICategoryRepository Categories { get; }
        public IPetRepository Pets { get; }
        public IUserRepository Users { get; }
        public IOrderRepository Orders { get; }
        public ICartRepository Carts { get; }
        public ICartItemRepository CartItems { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
