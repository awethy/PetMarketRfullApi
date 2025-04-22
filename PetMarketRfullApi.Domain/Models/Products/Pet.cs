using System.ComponentModel.DataAnnotations;

namespace PetMarketRfullApi.Domain.Models.Products
{
    public class Pet : Product
    {
        public DateOnly DateOfBirth { get; set; }
    }
}
