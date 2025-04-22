using AutoMapper;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.CartsResources;
using Microsoft.AspNetCore.Http.Metadata;

namespace PetMarketRfullApi.Application.Sevices
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResource> CreateCartAsync(CartRequest cartRequest)
        {
            var cart = new Cart();

            if (cartRequest.Items.Any())
            {
                cart.Items = _mapper.Map<List<CartItem>>(cartRequest.Items).ToList();
            }

            _unitOfWork.Carts.CreateCartAsync(cart);
            return await EnrichCart(cart);
        }

        public async Task<CartResource> FindByIdAsync(Guid id)
        {
            var cart = await _unitOfWork.Carts.GetCartByIdAsync(id);
            return await EnrichCart(cart);
        }

        public async Task<CartResource> EnrichCart(Cart cart)
        {
            if (cart == null) return null;

            foreach (var item in cart.Items)
            {
                var gotItem = await _unitOfWork.Pets.GetPetByIdAsync(item.Id);
                if (gotItem != null)
                {
                    item.UnitPrice = gotItem.Price;
                    item.Name = gotItem.Name;
                }
                else
                {
                    throw new Exception($"item with id = {item.Id} not found");
                }
            }
            return _mapper.Map<CartResource>(cart);
        }
    }
}
