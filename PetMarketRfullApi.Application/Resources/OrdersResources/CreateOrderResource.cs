using PetMarketRfullApi.Application.Resources.CartsResources;

namespace PetMarketRfullApi.Application.Resources.OrdersResources
{
    public class CreateOrderResource
    {
        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }

        public CartRequest? Cart { get; set; } = null!; 
    }
}
