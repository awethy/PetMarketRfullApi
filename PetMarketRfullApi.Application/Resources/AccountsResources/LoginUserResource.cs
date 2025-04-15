using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Application.Resources.AccountsResources
{
    public class LoginUserResource
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
