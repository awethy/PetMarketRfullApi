using PetMarketRfullApi.Application.Resources.OrdersResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IOrderService
    {
        Task<OrderResource> Create(CreateOrderResource createOrder);
        Task<OrderResource> GetByIdAsync(Guid orderId);
        Task<List<OrderResource>> GetByUser(string userId);
        Task<List<OrderResource>> GetAllAsync();
        Task Reject(Guid orderId);
    }
}
