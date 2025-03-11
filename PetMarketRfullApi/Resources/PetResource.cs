using PetMarketRfullApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources
{
    public class PetResource
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        public bool IsAvailable { get; set; }
        public CategoryResource Category { get; set; }
    }
}
