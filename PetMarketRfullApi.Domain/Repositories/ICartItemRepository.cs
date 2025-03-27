using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> AddRangeItemsAsync(List<CartItem> items);
    }
}
