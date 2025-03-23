using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Resources.OrdersResources;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order> GetById(Guid orderId);
        Task<List<Order>> GetByUser(string userId);
        Task<List<Order>> GetAll();
        Task Reject(Guid orderId);
    }
}
