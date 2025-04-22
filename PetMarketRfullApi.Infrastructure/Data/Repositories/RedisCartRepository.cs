using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.Cors.Infrastructure;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Infrastructure.Data.Contexts;
using StackExchange.Redis;

namespace PetMarketRfullApi.Infrastructure.Data.Repositories
{
    public class RedisCartRepository : BaseRepositories, IRedisCartRepository
    {
        private readonly IDatabase _database;

        public RedisCartRepository(IConnectionMultiplexer redis, AppDbContext appDbContext) : base(appDbContext)
        {
            _database = redis.GetDatabase();
        }

        public async Task SaveCartAsync(Guid id, IEnumerable<HashEntry> entries)
        {
            var cartKey = GetCartKey(id);
            foreach (var entry in entries)
            {
                await _database.HashSetAsync(cartKey, entry.Name, entry.Value);
            }
            await _database.KeyExpireAsync(cartKey, TimeSpan.FromMinutes(30));
        }

        public async Task DeleteAsync(Guid id)
        {
            var cartKey = GetCartKey(id);
            await _database.KeyDeleteAsync(cartKey);
        }


        //Проверка существования корзины. Использует Redis-команду EXISTS, которая возвращает:true – если ключ существует, false – если ключа нет
        public async Task<bool> ExistsAsync(Guid id)
        {
            var cartKey = GetCartKey(id);
            return await _database.KeyExistsAsync(cartKey);

        }

        public async Task<Cart> GetAsync(Guid id)
        {
            var cartKey = GetCartKey(id);
            var entries = await _database.HashGetAllAsync(cartKey);

            if (entries.Length == 0) return null;

            return new Cart
            {
                Id = id,
                Items = entries.Select(e => new CartItem
                {
                    Id = int.Parse(e.Name),
                    CartId = id,
                    Quantity = int.Parse(e.Value)
                }).ToList()
            };
        }

        private static string GetCartKey(Guid id) => $"cart:{id}";
    }
}
