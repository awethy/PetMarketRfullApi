using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Options;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;
using PetMarketRfullApi.Sevices;
using System.Data;
using System.Security.Claims;
using Xunit;

namespace PetMarketRfullApi.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IOptions<AuthOptions>> _mockAuthOptions;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);

            _mockMapper = new Mock<IMapper>();

            _mockAuthOptions = new Mock<IOptions<AuthOptions>>();
            _mockAuthOptions.Setup(x => x.Value)
                .Returns(new AuthOptions
                {
                    TokenPrivateKey = "TestSecretKeyWithMinimumLength32Characters",
                    ExpireIntervalMinutes = 60
                });

            _authService = new AuthService(
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockAuthOptions.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnSuccess()
        {
            //arrange
            var userResource = new LoginUserResource
            {
                Email = "test@example.com",
                Password = "Password123!",
                RememberMe = false
            };
            var user = new User
            {
                Id = "123",
                Email = userResource.Email,
                UserName = userResource.Password
            };
            _mockUserManager.Setup(x => x.FindByEmailAsync(userResource.Email)).ReturnsAsync(user);
            
            _mockSignInManager.Setup(x => x.PasswordSignInAsync(user, userResource.Password, userResource.RememberMe, false)).ReturnsAsync(SignInResult.Success);

            _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });

            //act
            var result = await _authService.LoginAsync(userResource);
            
            //assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.id);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.UserName, result.Name);
            Assert.Equal(new List<string> { "user" }, result.Roles);

            // Verify token was generated
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task LoginAsync_ReturnFailed_InvalidPass()
        {

        }

        [Fact]
        public async Task RegisterAsync_ReturnSuccess()
        {
            //arrange
            var createUserResource = new CreateUserResource
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Name = "testuser"
            };

            var user = new User
            {
                Id = "123",
                Email = createUserResource.Email,
                UserName = createUserResource.Name
            };

            var roles = new List<string> { "user" };

            // Setup mock sequence for FindByEmailAsync
            _mockUserManager.SetupSequence(x => x.FindByEmailAsync(createUserResource.Email))
            .ReturnsAsync((User)null) // First call - user doesn't exist
            .ReturnsAsync(user); // Second call - user found after creation

            // setup mapper
            _mockMapper.Setup(x => x.Map<User>(createUserResource))
                .Returns(user);

            // Setup successful user creation
            _mockUserManager.Setup(x => x.CreateAsync(user, createUserResource.Password))
                .ReturnsAsync(IdentityResult.Success);

            // setup successful role assignment
            _mockUserManager.Setup(x => x.AddToRoleAsync(user, "user"))
                .ReturnsAsync(IdentityResult.Success);

            // setup roles retrieval
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                    .ReturnsAsync(roles);

            //act
            var result = await _authService.RegisterUserAsync(createUserResource);

            //assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.id);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.UserName, result.Name);
            Assert.Equal(roles, result.Roles);

            // Verify token was generated
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }
    }
}
