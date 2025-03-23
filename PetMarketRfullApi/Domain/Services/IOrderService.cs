using PetMarketRfullApi.Resources.OrdersResources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IOrderService
    {
        Task<OrderResource> Create(CreateOrderResource createOrder);
        Task<OrderResource> GetByIdAsync(Guid orderId);
        Task<List<OrderResource>> GetByUser(string userId);
        Task<List<OrderResource>> GetAll();
        Task Reject(Guid orderId);
    }
}
