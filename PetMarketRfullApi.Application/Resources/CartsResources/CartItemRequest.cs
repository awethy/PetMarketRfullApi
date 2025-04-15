using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMarketRfullApi.Application.Resources.CartsResources
{
    public class CartItemRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
