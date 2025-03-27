using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetMarketRfullApi.Application.Resources.UsersResources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using PetMarketRfullApi.Application.Resources.AccountsResources;
using AutoMapper;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Domain.Options;

namespace PetMarketRfullApi.Application.Sevices
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly AuthOptions _authOptions;

        public AuthService(IMapper mapper,
                           UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IOptions<AuthOptions> authOptions)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _authOptions = authOptions.Value;
        }

        public async Task<UserResource> RegisterUserAsync(CreateUserResource createUserResource)
        {
            if (await _userManager.FindByEmailAsync(createUserResource.Email) != null)
            {
                throw new Exception($"Email {createUserResource.Email} already exists");
            }

                var userEn = _mapper.Map<User>(createUserResource);

                var createdUser = await _userManager.CreateAsync(userEn, createUserResource.Password);
                if (createdUser.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(createUserResource.Email) 
                    ?? throw new Exception($"User with email {createUserResource.Email} not registered");

                    var result = await _userManager.AddToRoleAsync(userEn, "user");
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var response = new UserResource
                        {
                            id = user.Id,
                            Email = user.Email,
                            Roles = userRoles.ToArray(),
                            Name = user.UserName,
                        };
                        return GenerateToken(response);
                    }
                    throw new Exception($"Errors: {string.Join(";", result.Errors
                        .Select(x => $"{x.Code} {x.Description}"))}");
                }
            throw new Exception();
        }

    public async Task<UserResource> LoginAsync(LoginUserResource userResource)
        {
            var user = await _userManager.FindByEmailAsync(userResource.Email) 
                ?? throw new Exception($"User with {userResource.Email} not found.");

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new Exception($"This account is locked!");
            }

            var result = await _signInManager.PasswordSignInAsync(user, userResource.Password, userResource.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var response = new UserResource
                {
                    id = user.Id,
                    Email = user.Email,
                    Roles = userRoles.ToArray(),
                    Name = user.UserName
                };
                return GenerateToken(response);
            }
            //else
            //{
            //    throw new Exception("Email or password incorrected!");
            //}
            throw new AuthenticationException();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public UserResource GenerateToken(UserResource userRegModel)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var claims = new Dictionary<string, Object>
            {
                {ClaimTypes.Name, userRegModel.Name!},
                {ClaimTypes.NameIdentifier, userRegModel.id!},
                {JwtRegisteredClaimNames.Aud, "test"},
                {JwtRegisteredClaimNames.Iss, "test"}
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(userRegModel),
                Expires = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalMinutes),
                SigningCredentials = credentials,
                Claims = claims,
                Audience = "test",
                Issuer = "test"
            };

            var token = handler.CreateToken(tokenDescriptor);
            userRegModel.Token = handler.WriteToken(token);

            return userRegModel;
        }

        private static ClaimsIdentity GenerateClaims(UserResource userRegModel)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, userRegModel.Email!));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegModel.id!));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));

            foreach (var role in userRegModel.Roles!)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}
