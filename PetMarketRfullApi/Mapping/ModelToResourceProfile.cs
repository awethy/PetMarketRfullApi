using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources;

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
            CreateMap<UpdateUserResource, User>();
        }
    }
}
