namespace PetMarketRfullApi.Resources.CartsResources
{
    public class CartResource
    {
        public Guid Id { get; set; }
        public List<CartItemResource> Items { get; set; } = null!;
    }
}
