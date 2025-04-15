using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface ICartService
    {
        Task<CartResource> FindByIdAsync(Guid id);
        Task<CartResource> CreateCartAsync(CartRequest cartRequest);
        Task<CartResource> EnrichCart(Cart cart);
    }
}
