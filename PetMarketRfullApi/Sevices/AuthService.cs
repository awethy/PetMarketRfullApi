using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;
using System.Security.Claims;

namespace PetMarketRfullApi.Sevices
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(CreateUserResource createUserResource)
        {
            try
            {
                var user = _mapper.Map<User>(createUserResource);
                var result = await _userManager.CreateAsync(user, createUserResource.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                }
                return result;
            }
            catch (Exception ex) { Console.WriteLine(ex); throw; }
        }

        public async Task<SignInResult> LoginAsync(LoginUserResource userResource)
        {
            var user = await _userManager.FindByEmailAsync(userResource.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return SignInResult.NotAllowed;
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            var result = await _signInManager.PasswordSignInAsync(user, userResource.Password, userResource.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                try
                {
                    var claims = new List<Claim>
                        {
                             new Claim(ClaimTypes.NameIdentifier, user.Id)
                        };

                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    await _userManager.AddClaimsAsync(user, claims);

                    await _signInManager.SignInAsync(user, userResource.RememberMe);
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    Console.WriteLine($"Ошибка при добавлении claims: {ex.Message}");
                    return SignInResult.Failed;
                }

            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordResource forgotPassword)
        //{
        //    var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        //    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        return 
        //    }

        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContent.Request.Scheme);
        //    EmailService email = EmailService();
        //}
    }
}
