﻿using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Repositories;

namespace PetMarketRfullApi.Data.Repositories
{
    public class CategoryRepository : BaseRepositories, ICategoryRepository
    {
        // Конструктор для внедрения зависимости через DI
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _appDbContext.Category.Add(category);
            await _appDbContext.SaveChangesAsync();
            return category;
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _appDbContext.Category.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _appDbContext.Category.FindAsync(id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _appDbContext.Category.FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
