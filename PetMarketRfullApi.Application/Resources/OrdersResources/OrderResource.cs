using PetMarketRfullApi.Application.Resources.CartsResources;

namespace PetMarketRfullApi.Application.Resources.OrdersResources
{
    public class OrderResource
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public CartResource Cart { get; set; } = null!;
    }
}
