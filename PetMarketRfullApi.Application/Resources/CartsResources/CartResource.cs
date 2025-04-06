using System.Diagnostics;

namespace PetMarketRfullApi.Application.Resources.CartsResources
{
    public class CartResource
    {
        public Guid Id { get; set; }
        public decimal Total => Items.Sum(x => x.SubTotal);
        public List<CartItemResource>? Items { get; set; } = new List<CartItemResource>();
    }
}
