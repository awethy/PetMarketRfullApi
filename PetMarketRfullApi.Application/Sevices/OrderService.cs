using AutoMapper;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.OrdersResources;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using StackExchange.Redis;
using System.Text.Json;

namespace PetMarketRfullApi.Application.Sevices
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICartService _cartService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDatabase _cache;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, ICartService cartService, IHttpContextAccessor accessor, IConnectionMultiplexer redisConnection)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _httpContextAccessor = accessor;
            _cache = redisConnection.GetDatabase();
        }

        public async Task<OrderResource> Create(CreateOrderResource createOrder)
        {
            var cart = await _cartService.CreateCartAsync(createOrder.Cart);
            if (cart.Id == Guid.Empty)
                throw new InvalidOperationException("Cart ID cannot be empty");

            var order = _mapper.Map<Domain.Models.OrderModels.Order>(createOrder);

            order.CartId = cart.Id;
            order.UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var createdOrder = await _unitOfWork.Orders.AddOrderAsync(order);
            // Удаляем кэш, чтобы при следующем запросе данные обновились
            await _cache.KeyDeleteAsync("orders_all");

            return _mapper.Map<OrderResource>(createdOrder);
        }

        public async Task<List<OrderResource>> GetAllAsync()
        {
            //Пытаемся получить данные из кэша
            var cachedData = await _cache.StringGetAsync("orders_all");
            if (cachedData.HasValue)
            {
                //Если данные есть в кэше — десериализуем и возвращаем
                return JsonSerializer.Deserialize<List<OrderResource>>(cachedData!);
            }

            var orders = await _unitOfWork.Orders.GetAllAsync();
            var result = _mapper.Map<List<OrderResource>>(orders);

            //Сохраняем в Redis (сериализуем в JSON) и Кэшируем на 10 минут
            await _cache.StringSetAsync(
                "orders_all",
                JsonSerializer.Serialize(result),
                TimeSpan.FromMinutes(10)
            );

            return result;
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
