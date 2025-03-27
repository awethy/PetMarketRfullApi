using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Application.Resources.СategoriesResources
{
    public class CreateCategoryResource
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
