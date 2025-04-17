using PetMarketRfullApi.Application.Resources.CartsResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IRedisCartService
    {
        Task<CartResource> GetCartById(Guid id);
        Task<CartResource> CreateCartAsync(CartRequest request);
        Task<CartResource> UpdateCartAsync(Guid id, CartRequest request);
        Task DeleteCartAsync(Guid id);
        Task<bool> ExistsCartAsync(Guid id);
    }
}
