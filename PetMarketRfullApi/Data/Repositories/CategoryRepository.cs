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

        public Task AddCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _appDbContext.Category.ToListAsync();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
