using PetMarketRfullApi.Resources.CartsResources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface ICartService
    {
        Task<CartResource> CreateCartAsync(CartResource cart);
    }
}
