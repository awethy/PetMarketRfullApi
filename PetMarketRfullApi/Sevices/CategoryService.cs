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

        public async Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryResource>>(categories);
        }

        public async Task<CategoryResource> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new InvalidOperationException("Not found category");
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

        public async Task UpdateCategoryAsync(int id, UpdateCategoryResource updateCategoryResource)
        {
            var existingCategory = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            //проверяем, существует ли категория с таким же именем (кроме текущей)
            var categoryWithSameName = await _unitOfWork.Categories.GetByNameAsync(updateCategoryResource.Name);
            if (categoryWithSameName != null && categoryWithSameName.Id != id)
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }

            existingCategory.Name = updateCategoryResource.Name;
            existingCategory.Description = updateCategoryResource.Description;

            var category = _mapper.Map<Category>(existingCategory);

            await _unitOfWork.Categories.UpdateCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var existingCategory = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new InvalidOperationException("Not found category");
            }
            else if (existingCategory.Pets.Any())
            {
                // Если есть связанные продукты, выбрасываем исключение или возвращаем ошибку
                throw new InvalidOperationException("Категория не может быть удалена, так как у нее есть связанные pets.");
            }
            await _unitOfWork.Categories.DeleteCategoryAsync(id);
        }
    }
}
