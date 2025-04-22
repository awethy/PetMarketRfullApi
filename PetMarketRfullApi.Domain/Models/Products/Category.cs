using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Domain.Models.Products
{
    public class Category
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public ICollection<OtherItem> OtherItems { get; set; }
    }
}
