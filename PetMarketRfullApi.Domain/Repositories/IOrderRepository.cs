using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order> GetById(Guid orderId);
        Task<List<Order>> GetByUserAsync(string userId);
        Task<List<Order>> GetAllAsync();
        Task Reject(Guid orderId);
    }
}
