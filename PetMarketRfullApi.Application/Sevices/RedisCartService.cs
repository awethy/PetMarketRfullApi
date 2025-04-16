using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Application.Sevices
{
    public class RedisCartService : IRedisCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;

        public RedisCartService(IUnitOfWork unitOfWork, IPetService petService)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
        }
// TODO: Сделать сервисные CRUD'ы Create, Update, Delete, Exists с проверками данных 
        public Task<CartResource> CreateCartAsync(CartRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCartAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<CartResource> GetCartById(Guid id)
        {
            var cart = await _unitOfWork.RedisCarts.GetAsync(id);

            if (cart == null) return null;
            if (cart.Items == null) throw new ArgumentNullException(nameof(cart.Items));

            // Обогащаем данные о товарах
            var items = new List<CartItemResource>();
            foreach (var item in cart.Items)
            {
                var product = await _petService.GetPetByIdAsync(item.ItemId);
                if (product == null) continue;

                items.Add(new CartItemResource
                {
                    Id = item.ItemId,
                    Name = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            return new CartResource
            {
                Id = id,
                Items = items
            };
        }

        public Task<CartResource> UpdateCartAsync(Guid id, CartRequest request)
        {
            throw new NotImplementedException();
        }

        public Task ValidateCartItems(IEnumerable<CartItemRequest> items)
        {
            throw new NotImplementedException();
        }
    }
}
