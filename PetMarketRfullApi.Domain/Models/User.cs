using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Domain.Models
{
    public class User : IdentityUser
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public string Id { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}