using PetMarketRfullApi.Resources.CartsResources;

namespace PetMarketRfullApi.Resources.OrdersResources
{
    public class OrderResource
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; }
        public CartResource Cart { get; set; } = null!;
    }
}
