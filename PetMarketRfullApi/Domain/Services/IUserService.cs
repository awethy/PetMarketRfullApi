using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IUserService
    {
        // Получить все user
        Task<IEnumerable<UserResource>> GetAllUsersAsync();

        // Получить user по id
        Task<UserResource> GetUserByIdAsync(int id);

        // Добавить новую user
        Task<UserResource> CreateUserAsync(CreateUserResource createUserResource);

        // Обновить существующую user
        Task UpdateUserAsync(int id, UpdateUserResource updateUserResource);

        // Удалить user по id
        Task DeleteUserAsync(int id);
    }
}
