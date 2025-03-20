using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources.UsersResources
{
    public class UpdateUserResource
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
