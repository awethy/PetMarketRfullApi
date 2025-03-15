using PetMarketRfullApi.Domain.Models;

namespace PetMarketRfullApi.Domain.Repositories
{
    public interface IUserRepository
    {
        // Получить все user
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Получить user по id
        Task<User> GetUserByIdAsync(int id);

        // Добавить новую user
        Task<User> AddUserAsync(User user);

        // Обновить существующую user
        Task UpdateUserAsync(User user);

        // Удалить user по id
        Task DeleteUserAsync(int id);
        Task<User> GetByNameAsync(string name);
    }
}
