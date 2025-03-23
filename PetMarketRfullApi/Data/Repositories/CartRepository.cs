using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class CartRepository : BaseRepositories, ICartRepository
    {
        // Конструктор для внедрения зависимости через DI
        public CartRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            _appDbContext.Carts.Add(cart);
            await _appDbContext.SaveChangesAsync();
            return cart;
        }
    }
}
