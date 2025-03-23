namespace PetMarketRfullApi.Resources.CartsResources
{
    public class CartItemResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid CartId { get; set; }
    }
}
