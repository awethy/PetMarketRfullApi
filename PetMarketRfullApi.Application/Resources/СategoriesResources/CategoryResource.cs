using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Application.Resources.СategoriesResources
{
    public class CategoryResource
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> PetNames { get; set; }
    }
}
