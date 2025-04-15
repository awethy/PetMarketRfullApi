using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.OrdersResources;

namespace PetMarketRfullApi.Web.Controllers
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUserAsync(string userId)
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllAsync();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
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
