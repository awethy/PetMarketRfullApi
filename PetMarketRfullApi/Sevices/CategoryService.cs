using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Data.Contexts;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Services;

namespace PetMarketRfullApi.Sevices
{
    public class CategoryService : ICategoryServices
    {
        //Контекст БД

        private readonly AppDbContext _appDbContext;

        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _appDbContext.Category.ToListAsync();
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> CreateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
