using AutoMapper;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.OrdersResources;

namespace PetMarketRfullApi.Sevices
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResource> Create(CreateOrderResource createOrder)
        {
            var order = _mapper.Map<Order>(createOrder);
            var createdOrder = await _unitOfWork.Orders.AddOrderAsync(order);
            return _mapper.Map<OrderResource>(createdOrder);
        }

        public Task<List<OrderResource>> GetAll()
        {
            throw new NotImplementedException();
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

        public Task<List<OrderResource>> GetByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task Reject(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
