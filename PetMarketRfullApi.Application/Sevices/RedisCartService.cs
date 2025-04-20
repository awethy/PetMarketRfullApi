using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Domain.Models.OrderModels;
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
            ValidateCart(request, r => r.Items, item => item.Quantity);

            var entries = ConvertToHashEntries(request.Items);

            Guid id = Guid.NewGuid();
            
            await _unitOfWork.RedisCarts.SaveCartAsync(id, entries);

            // Обогащаем данные о товарах
            var items = await MapItemsToResourcesAsync(request.Items, item => item.Id, item => item.Quantity);

            return new CartResource { Id = id, Items = items };
        }

        public async Task<CartResource> UpdateCartAsync(Guid id, CartRequest request)
        {
            ValidateCart(request, r => r.Items, item => item.Quantity);

            var entries = new List<HashEntry>();
            foreach (var item in request.Items)
            {
                entries.Add(new HashEntry(
                        item.Id.ToString(),
                        item.Quantity.ToString()
                    ));
            }

            await _unitOfWork.RedisCarts.SaveCartAsync(id, entries);

            var items = await MapItemsToResourcesAsync(request.Items, item => item.Id, item => item.Quantity);

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

            ValidateCart(cart, c => c.Items, item => item.Quantity);

            var items = await MapItemsToResourcesAsync(cart.Items, item => item.ItemId, item => item.Quantity);

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
        private static void ValidateCart<TCart, TItem>(TCart cart, 
            Func<TCart, IEnumerable<TItem>> getItems,
            Func<TItem, int> getQuantityFunc)
            where TCart : class
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            var items = getItems(cart);
            if (items == null) throw new ArgumentException("Cart items collection cannot be null", nameof(items));

            //проверка каждого товара
            foreach ( var item in items)
            {
                var quantity = getQuantityFunc(item);
                if (quantity == 0 || quantity <= 0)
                {
                    throw new Exception($"Invalid quantity ({quantity}) for item ({item}), nameof({items})");
                }
            }
        }

        //Маппер List<CartItemRequest> to List<CartItemResource>
        private async Task<List<CartItemResource>> MapItemsToResourcesAsync<T>(
            IEnumerable<T> items,
            Func<T, int> getId,
            Func<T, int> getQuantity) 
        {
            var result = new List<CartItemResource>();
            foreach (var item in items)
            {
                var id = getId(item);
                var product = await _petService.GetPetByIdAsync(id);
                if (product == null) continue;

                result.Add(new CartItemResource
                {
                    Id = id,
                    Name = product.Name,
                    Quantity = getQuantity(item),
                    UnitPrice = product.Price
                });
            }
            return result;
        }

        //Конвертация list<CartItemRequest> в hash для сохранении в redis бд
        // НЕ работает
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
