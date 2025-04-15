using PetMarketRfullApi.Application.Resources.СategoriesResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResource>> GetAllCategoriesAsync();
        Task<CategoryResource> GetCategoryByIdAsync(int id);
        Task<CategoryResource> CreateCategoryAsync(CreateCategoryResource createCategoryResource);
        Task UpdateCategoryAsync(int id, UpdateCategoryResource updateCategoryResource);
        Task DeleteCategoryAsync(int id);
    }
}
