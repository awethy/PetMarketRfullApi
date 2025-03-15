using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources
{
    public class CreateUserResource
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(18)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
