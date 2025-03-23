namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public List<CartItem>? Items { get; set; }
    }
}
