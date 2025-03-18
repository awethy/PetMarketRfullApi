using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;

namespace PetMarketRfullApi.Domain.Services
{
    public interface IAuthService
    {
        Task<SignInResult> LoginAsync(LoginUserResource userResource);

        Task LogoutAsync();

        Task<IdentityResult> RegisterUserAsync(CreateUserResource createUserResource);
    }
}
