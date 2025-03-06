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

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllCategoriesAsync();
        }

        public async Task<CategoryResource> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
            {   
                return null;
            }
            return _mapper.Map<CategoryResource>(category);
        }

        public async Task<CategoryResource> CreateCategoryAsync(CreateCategoryResource createCategoryResource)
        {             
            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(createCategoryResource.Name);
            if (existingCategory != null)
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }


            var category = _mapper.Map<Category>(createCategoryResource);
            var createdCategory = await _unitOfWork.Categories.AddCategoryAsync(category);
            return _mapper.Map<CategoryResource>(createdCategory);
        }

        public Task<CategoryResource> UpdateCategoryAsync(int id, Category category)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var existingCategory = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new InvalidOperationException("Not found category");
            }
            //if (existingCategory.Pets.Any())
            //{
            //    throw new InvalidOperationException("Cannot delete category with associated pets.");
            //}
            await _unitOfWork.Categories.DeleteCategoryAsync(id);
        }
    }
}
