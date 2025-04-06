using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMarketRfullApi.Application.Resources.CartsResources
{
    public class CartRequest
    {
        public List<CartItemResource> Items { get; set; } = new List<CartItemResource>(); 
    }
}
