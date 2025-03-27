using PetMarketRfullApi.Application.Resources.CartsResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface ICartService
    {
        Task<CartResource> CreateCartAsync(CartResource cart);
    }
}
