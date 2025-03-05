using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Sevices
{
    public class CategoryService : ICategoryServices
    {
        //Контекст Репозитория

        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repository.GetAllCategoriesAsync();
        }

        public async Task<CategoryResource> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);
            if (category == null)
            {   
                return null;
            }
            return _mapper.Map<CategoryResource>(category);
        }

        public async Task<CategoryResource> CreateCategoryAsync(CreateCategoryResource createCategoryResource)
        {             
            var existingCategory = await _repository.GetByNameAsync(createCategoryResource.Name);
            if (existingCategory != null)
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }


            var category = _mapper.Map<Category>(createCategoryResource);
            var createdCategory = await _repository.AddCategoryAsync(category);
            return _mapper.Map<CategoryResource>(createdCategory);
        }

        public Task<CategoryResource> UpdateCategoryAsync(int id, Category category)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var existingCategory = await _repository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new InvalidOperationException("Not found category");
            }
            //if (existingCategory.Pets.Any())
            //{
            //    throw new InvalidOperationException("Cannot delete category with associated pets.");
            //}
            await _repository.DeleteCategoryAsync(id);
        }
    }
}
