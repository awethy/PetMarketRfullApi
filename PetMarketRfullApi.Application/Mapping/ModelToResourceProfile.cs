using AutoMapper;
using PetMarketRfullApi.Application.Resources.CartsResources;
using PetMarketRfullApi.Application.Resources.OrdersResources;
using PetMarketRfullApi.Application.Resources.PetsResources;
using PetMarketRfullApi.Application.Resources.UsersResources;
using PetMarketRfullApi.Application.Resources.СategoriesResources;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Models.OrderModels;

namespace PetMarketRfullApi.Application.Mapping
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
            CreateMap<Order, OrderResource>()
                .ForPath(dest=> dest.Cart.Items, opt => opt.MapFrom(src => src.Cart.Items));

            CreateMap<CreateOrderResource, Order>();
            CreateMap<Order, CreateOrderResource>();

            CreateMap<Cart, CartResource>();

            CreateMap<CartItemRequest, CartItem>().ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id));
            CreateMap<CartItem, CartItemResource>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId));

            CreateMap<Cart, CartRequest>();
        }
    }
}
