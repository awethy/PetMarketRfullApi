using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Application.Resources.AccountsResources
{
    public class ForgotPasswordResource
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
