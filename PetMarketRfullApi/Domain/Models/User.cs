using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Domain.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}