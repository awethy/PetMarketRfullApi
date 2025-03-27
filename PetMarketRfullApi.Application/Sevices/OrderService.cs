using AutoMapper;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.OrdersResources;

namespace PetMarketRfullApi.Application.Sevices
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICartService _cartService;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, ICartService cartService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        public async Task<OrderResource> Create(CreateOrderResource createOrder)
        {
            var cart = await _cartService.CreateCartAsync(createOrder.Cart);
            if (cart.Id == Guid.Empty)
                throw new InvalidOperationException("Cart ID cannot be empty");

            var order = _mapper.Map<Order>(createOrder, opts =>
            {
                opts.Items["CartId"] = cart.Id;
            });
            order.CartId = cart.Id;

            var createdOrder = await _unitOfWork.Orders.AddOrderAsync(order);

            return _mapper.Map<OrderResource>(createdOrder);
        }

        public async Task<List<OrderResource>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();

            return _mapper.Map<List<OrderResource>>(orders);
        }

        public async Task<OrderResource> GetByIdAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Not found order");
            }
            return _mapper.Map<OrderResource>(order);
        }

        public async Task<List<OrderResource>> GetByUser(string userId)
        {
            var orders = await _unitOfWork.Orders.GetByUserAsync(userId);

            return _mapper.Map<List<OrderResource>>(orders);
        }

        public Task Reject(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
