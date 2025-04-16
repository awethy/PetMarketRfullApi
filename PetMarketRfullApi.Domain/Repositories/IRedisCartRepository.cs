using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IRedisCartRepository
    {
        Task<Cart> GetAsync(Guid id);
        Task CreateOrUpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
