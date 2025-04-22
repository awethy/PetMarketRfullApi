using FluentValidation;
using PetMarketRfullApi.Domain.Models.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class CartItem
    {
        [Required]
        public int Id { get; set; }
        public Product Product { get; set; }

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
}
