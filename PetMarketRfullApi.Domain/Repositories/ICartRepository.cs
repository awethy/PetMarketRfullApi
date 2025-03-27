using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> CreateCartAsync(Cart cart);
    }
}
