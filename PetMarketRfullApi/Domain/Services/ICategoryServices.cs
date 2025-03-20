using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources.СategoriesResource;

namespace PetMarketRfullApi.Domain.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync();
        Task<CategoryResource> GetCategoryByIdAsync(int id);
        Task<CategoryResource> CreateCategoryAsync(CreateCategoryResource createCategoryResource);
        Task UpdateCategoryAsync(int id, UpdateCategoryResource updateCategoryResource);
        Task DeleteCategoryAsync(int id);
    }
}
