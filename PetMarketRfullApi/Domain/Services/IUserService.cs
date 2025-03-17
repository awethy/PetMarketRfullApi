using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Resources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IUserService
    {
        // Получить все user
        Task<IEnumerable<UserResource>> GetAllUsersAsync();

        // Получить user по id
        Task<UserResource> GetUserByIdAsync(string id);

        // Добавить новую user
        //Task<UserResource> CreateUserAsync(CreateUserResource createUserResource);

        Task<SignInResult> LoginAsync(LoginUserResource userResource);

        Task LogoutAsync();

        Task<IdentityResult> RegisterUserAsync(CreateUserResource createUserResource);

        // Обновить существующую user
        Task UpdateUserAsync(string id, UpdateUserResource updateUserResource);

        // Удалить user по id
        Task DeleteUserAsync(string id);
    }
}
