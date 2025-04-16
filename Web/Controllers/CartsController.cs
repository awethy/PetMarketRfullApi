using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Infrastructure.Data.Repositories;

namespace PetMarketRfullApi.Web.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsController : Controller
    {
        private readonly IRedisCartService _cartService;

        public CartsController(IRedisCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("Redis-{id}")]
        public async Task<ActionResult<CartResource>> RedisGetCart(Guid id)
        {
            var cart = await _cartService.GetCartById(id);
            return cart != null ? Ok(cart) : NotFound();
        }

        // TODO: Добавить CRUD'ы - Create, Update, Delete, Exists
    }
}
