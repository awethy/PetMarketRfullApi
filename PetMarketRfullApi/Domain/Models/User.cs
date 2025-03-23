using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Domain.Models.OrderModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetMarketRfullApi.Domain.Models
{
    public class User : IdentityUser
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public string Id { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}