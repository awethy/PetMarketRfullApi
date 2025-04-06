using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }

        public int ItemId { get; set; }
        public Pet Pet { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
}
