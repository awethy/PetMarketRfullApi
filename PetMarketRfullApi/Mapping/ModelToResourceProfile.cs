using AutoMapper;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {

            CreateMap<Category, CategoryResource>();
            CreateMap<CategoryResource, Category>();
            CreateMap<CreateCategoryResource, Category>();
            CreateMap<UpdateCategoryResource, Category>();

            CreateMap<Pet, PetResource>();
            CreateMap<PetResource, Pet>();
            CreateMap<CreatePetResource, Pet>();
            CreateMap<UpdatePetResource, Pet>();
        }
    }
}
