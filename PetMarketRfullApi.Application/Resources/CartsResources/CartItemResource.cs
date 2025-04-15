using PetMarketRfullApi.Domain.Models;

namespace PetMarketRfullApi.Application.Resources.CartsResources
{
    public class CartItemResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
    }
}
