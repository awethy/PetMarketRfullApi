using PetMarketRfullApi.Infrastructure.Data.Contexts;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace PetMarketRfullApi.Infrastructure.Data.Repositories
{
    public class CartRepository : BaseRepositories, ICartRepository
    {
        // Конструктор для внедрения зависимости через DI
        public CartRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            return await _appDbContext.Carts
                                    .Include(e => e.Items)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            _appDbContext.Carts.Add(cart);
            await _appDbContext.SaveChangesAsync();
            return cart;
        }
    }
}
