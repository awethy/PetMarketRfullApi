using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class CartItemRepository : BaseRepositories, ICartItemRepository
    {
        // Конструктор для внедрения зависимости через DI
        public CartItemRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<CartItem>> AddRangeItemsAsync(List<CartItem> items)
        {
            await _appDbContext.AddRangeAsync(items);
            await _appDbContext.SaveChangesAsync();
            return items;
        }
    }
}
