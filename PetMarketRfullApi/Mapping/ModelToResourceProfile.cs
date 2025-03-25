﻿using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Models.OrderModels;
using PetMarketRfullApi.Resources.CartsResources;
using PetMarketRfullApi.Resources.OrdersResources;
using PetMarketRfullApi.Resources.PetsResources;
using PetMarketRfullApi.Resources.UsersResources;
using PetMarketRfullApi.Resources.СategoriesResource;

namespace PetMarketRfullApi.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {

            CreateMap<Category, CategoryResource>()
                .ForMember(dest => dest.PetNames, opt => opt.MapFrom(src => src.Pets.Select(p => p.Name).ToList()));
            CreateMap<CategoryResource, Category>();
            CreateMap<CreateCategoryResource, Category>();
            CreateMap<UpdateCategoryResource, Category>();

            CreateMap<Pet, PetResource>();
            CreateMap<PetResource, Pet>();
            CreateMap<CreatePetResource, Pet>();
            CreateMap<UpdatePetResource, Pet>();

            CreateMap<User, UserResource>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));
            CreateMap<UserResource, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        
            CreateMap<CreateUserResource, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            CreateMap<UpdateUserResource, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<OrderResource, Order>();
            CreateMap<Order, OrderResource>();
            CreateMap<CreateOrderResource, Order>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom((src, dest, _, context) => context.Items["CartId"]));
            CreateMap<Order, CreateOrderResource>();

            CreateMap<CartResource, Cart>();
            CreateMap<Cart, CartResource>();

            CreateMap<CartItemResource, CartItem>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom((src, dest, _, context) => context.Items["CartId"]));
        }
    }
}
