using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources.CartsResources;

namespace PetMarketRfullApi.Resources.OrdersResources
{
    public class CreateOrderResource
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public CartResource Cart { get; set; } = null!; 
    }
}
