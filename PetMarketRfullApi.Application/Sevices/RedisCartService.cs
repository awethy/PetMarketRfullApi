using AutoMapper;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Domain.Repositories;
using StackExchange.Redis;

namespace PetMarketRfullApi.Application.Sevices
{
    public class RedisCartService : IRedisCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        public RedisCartService(IUnitOfWork unitOfWork, IPetService petService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
            _mapper = mapper;
        }

        public async Task<CartResource> CreateCartAsync(CartRequest request)
        {
            ValidateCart(request);

            var entries = ConvertToHashEntries(request.Items);

            Guid id = Guid.NewGuid();
            
            await _unitOfWork.RedisCarts.SaveCartAsync(id, entries);

            // Обогащаем данные о товарах
            var items = await MapReqItemToItemResourceAsync(request.Items);

            return new CartResource { Id = id, Items = items };
        }

        public async Task<CartResource> UpdateCartAsync(Guid id, CartRequest request)
        {
            ValidateCart(request);

            var entries = ConvertToHashEntries(request.Items);

            await _unitOfWork.RedisCarts.SaveCartAsync(id, entries);

            var items = await MapReqItemToItemResourceAsync(request.Items);

            return new CartResource { Id = id, Items = items };
        }

        public async Task DeleteCartAsync(Guid id)
        {
            var cart = await _unitOfWork.RedisCarts.GetAsync(id);
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            await _unitOfWork.RedisCarts.DeleteAsync(id);
        }

        public async Task<CartResource> GetCartById(Guid id)
        {
            var cart = await _unitOfWork.RedisCarts.GetAsync(id);

            var request = _mapper.Map<CartRequest>(cart);

            ValidateCart(request);

            // Обогащаем данные о товарах
            var items = await MapReqItemToItemResourceAsync(request.Items);

            return new CartResource { Id = id, Items = items };
        }

        //Проверка на наличие cart
        public async Task<bool> ExistsCartAsync(Guid id)
        {
            var cart = await _unitOfWork.RedisCarts.GetAsync(id);
            if (cart == null) return false;
            return true;
        }



        //Проверка корзины на ниличие и на наличие позиций корзины
        private static void ValidateCart(CartRequest cartRequest)
        {
            if (cartRequest == null) throw new ArgumentNullException(nameof(cartRequest));

            if (cartRequest.Items == null) throw new ArgumentException("Cart items collection cannot be null", nameof(cartRequest.Items));
        }

        //Маппер List<CartItemRequest> to List<CartItemResource>
        private async Task<List<CartItemResource>> MapReqItemToItemResourceAsync(IEnumerable<CartItemRequest> reqItems)
        {
            var items = new List<CartItemResource>();
            foreach (var item in reqItems)
            {
                var product = await _petService.GetPetByIdAsync(item.Id);
                if (product == null) continue;

                items.Add(new CartItemResource
                {
                    Id = item.Id,
                    Name = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }
            return items;
        }

        //Конвертация list<CartItemRequest> в hash для сохранении в redis бд
        private static IEnumerable<HashEntry> ConvertToHashEntries(IEnumerable<CartItemRequest> items)
        {
            return items?
                .Where(i => i != null)
                .Select(i => new HashEntry (
                    i.Id.ToString(),
                    i.Quantity.ToString()
                )) ?? Enumerable.Empty<HashEntry>();
        }
    }
}
