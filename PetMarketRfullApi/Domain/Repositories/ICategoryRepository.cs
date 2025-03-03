using PetMarketRfullApi.Domain.Models;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface ICategoryRepository
    {
        // Получить все категории
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        // Получить категорию по идентификатору
        Task<Category> GetCategoryByIdAsync(int id);

        // Добавить новую категорию
        Task AddCategoryAsync(Category category);

        // Обновить существующую категорию
        Task UpdateCategoryAsync(Category category);

        // Удалить категорию по идентификатору
        Task DeleteCategoryAsync(int id);
    }
}
