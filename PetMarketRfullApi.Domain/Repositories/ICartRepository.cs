using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<Cart> CreateCartAsync(Cart cart);
    }
}
