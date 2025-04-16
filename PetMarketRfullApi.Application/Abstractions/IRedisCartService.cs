using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetMarketRfullApi.Application.Resources.CartsResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IRedisCartService
    {
        Task<CartResource> GetCartById(Guid id);
        Task<CartResource> CreateCartAsync(CartRequest request);
        Task<CartResource> UpdateCartAsync(Guid id, CartRequest request);
        Task DeleteCartAsync(Guid id);
        Task ValidateCartItems(IEnumerable<CartItemRequest> items);
    }
}
