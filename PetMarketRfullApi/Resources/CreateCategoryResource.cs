using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources
{
    public class CreateCategoryResource
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
