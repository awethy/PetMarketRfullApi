using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources.СategoriesResource
{
    public class UpdateCategoryResource
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
