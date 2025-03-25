using AutoMapper;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.CartsResources;
using System.Net.Http.Headers;

namespace PetMarketRfullApi.Sevices
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResource> CreateCartAsync(CartResource cartResource)
        {
            var cart = _mapper.Map<Cart>(cartResource);
            // Создание корзины и сохранение в базу данных
            var cartSaveResult = await _unitOfWork.Carts.CreateCartAsync(cart);

            // Маппинг CartItemResource в CartItem и установка CartId в контекст маппинга
            var cartItems = _mapper.Map<List<CartItem>>(cartResource.Items, opts =>
            {
                opts.Items["CartId"] = cartSaveResult.Id;
            });

            // Добавление элементов корзины в базу данных
            await _unitOfWork.CartItems.AddRangeItemsAsync(cartItems);

            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<CartResource>(cart);
        }
    }
}
