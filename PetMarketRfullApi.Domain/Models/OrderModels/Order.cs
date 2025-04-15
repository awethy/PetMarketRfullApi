using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Domain.Models.OrderModels
{
    public class Order
    {
        [Required]
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "IsActive";

        public string? UserId { get; set; }
        public User? User { get; set; }

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; } 

    }
}
