using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;

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

        [HttpPost("Redis-create")]
        public async Task<ActionResult<CartResource>> RedisCreateCart(CartRequest request)
        {
            var cart = await _cartService.CreateCartAsync(request);
            return cart == null ? NotFound() : Ok(cart);
        }

        [HttpPost("Redis-update{id}")]
        public async Task<ActionResult<CartResource>> RedisUpdateCart(Guid id, CartRequest request)
        {
            var cart = await _cartService.UpdateCartAsync(id, request);
            return cart == null ? NotFound() : Ok(cart);
        }

        [HttpDelete("Redis-delete{id}")]
        public async Task<IActionResult> RedisDeleteCart(Guid id)
        {
            try
            {
                await _cartService.DeleteCartAsync(id);
                return Ok();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpGet("Redis-exists{id}")]
        public async Task<IActionResult> ExistsCart(Guid id)
        {
            var haveCart = await _cartService.ExistsCartAsync(id);
            if (haveCart == false)
            {
                return NotFound($"Cart с id-{id} не найден");
            }
            return Ok($"Cart с id-{id} есть в кэше Redis");
        }
    }
}
