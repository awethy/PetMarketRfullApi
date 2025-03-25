using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class OrderRepository : BaseRepositories, IOrderRepository
    {
        // Конструктор для внедрения зависимости через DI
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            _appDbContext.Orders.Add(order);
            await _appDbContext.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _appDbContext.Orders
                .Include(o => o.Cart)
                .ThenInclude(c => c.Items)
                .ToListAsync();
        }

        public async Task<Order> GetById(Guid orderId)
        {
            return await _appDbContext.Orders.FindAsync(orderId);
        }

        public async Task<List<Order>> GetByUserAsync(string userId)
        {
            return await _appDbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public Task Reject(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
