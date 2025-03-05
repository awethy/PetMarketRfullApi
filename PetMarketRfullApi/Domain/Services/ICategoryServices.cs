using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<CategoryResource> GetCategoryByIdAsync(int id);
        Task<CategoryResource> CreateCategoryAsync(CreateCategoryResource createCategoryResource);
        Task<Category> UpdateCategoryAsync(int id, Category category);
        Task<Category> DeleteCategoryAsync(int id);
    }
}
