using PetMarketRfullApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources
{
    public class CategoryResource
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    }
}
