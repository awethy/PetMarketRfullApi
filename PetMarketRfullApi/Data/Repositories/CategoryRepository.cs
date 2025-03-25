using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _appDbContext.Category.FindAsync(id);
            if (category != null)
            {
                _appDbContext.Category.Remove(category);
                await _appDbContext.SaveChangesAsync();
            } 
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _appDbContext.Category
                .Include(c => c.Pets)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _appDbContext.Category.FindAsync(id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _appDbContext.Category
                .Include(c => c.Pets)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            _appDbContext.Category.Update(category);
            return Task.CompletedTask;
        }
    }
}
