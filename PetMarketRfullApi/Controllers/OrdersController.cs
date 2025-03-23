using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.OrdersResources;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Policy = "CreateOrder")]
        public async Task<ActionResult<OrderResource>> AddOrder(CreateOrderResource createOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderResource = await _orderService.Create(createOrder);
            return CreatedAtAction(nameof(GetOrder), new { id = orderResource.Id }, orderResource);

        }
    }
}
