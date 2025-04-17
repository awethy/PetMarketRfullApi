using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetMarketRfullApi.Domain.Models.OrderModels;
using StackExchange.Redis;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IRedisCartRepository
    {
        Task<Cart> GetAsync(Guid id);
        Task SaveCartAsync(Guid id, IEnumerable<HashEntry> entries);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
