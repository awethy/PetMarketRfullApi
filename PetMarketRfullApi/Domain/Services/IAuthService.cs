using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IAuthService
    {
        Task<UserResource> LoginAsync(LoginUserResource userResource);

        Task LogoutAsync();

        Task<UserResource> RegisterUserAsync(CreateUserResource createUserResource);
    }
}
