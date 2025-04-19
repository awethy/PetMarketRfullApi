using PetMarketRfullApi.Infrastructure.Data.Contexts;
using PetMarketRfullApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace PetMarketRfullApi.Infrastructure.Data.Repositories
{
    public class UnitOfWork : BaseRepositories, IUnitOfWork
    {
        private readonly IConnectionMultiplexer _redis;

        public UnitOfWork(IConnectionMultiplexer redis , AppDbContext appDbContext) : base(appDbContext) 
        {
            _redis = redis;

            RedisCarts = new RedisCartRepository(_redis, appDbContext);
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
        public IRedisCartRepository RedisCarts { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
