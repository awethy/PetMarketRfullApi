using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Application.Resources.AccountsResources;
using PetMarketRfullApi.Application.Resources.UsersResources;

namespace PetMarketRfullApi.Application.Abstractions
{
    public interface IAuthService
    {
        Task<UserResource> LoginAsync(LoginUserResource userResource);

        Task LogoutAsync();

        Task<UserResource> RegisterUserAsync(CreateUserResource createUserResource);
    }
}
