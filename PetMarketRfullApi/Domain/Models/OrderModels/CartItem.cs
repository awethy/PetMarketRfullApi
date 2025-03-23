namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
