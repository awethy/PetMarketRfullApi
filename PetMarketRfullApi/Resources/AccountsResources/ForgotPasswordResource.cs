using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Resources.AccountResources
{
    public class ForgotPasswordResource
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
