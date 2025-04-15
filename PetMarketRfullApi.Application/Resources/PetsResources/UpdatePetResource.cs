using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Application.Resources.PetsResources
{
    public class UpdatePetResource
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateOnly DateOfBirth { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
    }
}
