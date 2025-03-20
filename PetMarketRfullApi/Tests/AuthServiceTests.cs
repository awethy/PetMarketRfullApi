using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using PetMarketRfullApi.Domain.Models;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Sevices;
using System.Security.Claims;
using Xunit;

namespace PetMarketRfullApi.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task LoginAsync_ReturnSuccess()
        {
            //arrange
            var user = new User { Email = "UTest@examle.com"/*Id = "657bf307-50a0-4742-9dd5-118b8f9cafcb"*/ };
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync("UTest@examle.com")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsEmailConfirmedAsync(user)).ReturnsAsync(false);
            userManagerMock.Setup(x => x.IsLockedOutAsync(user)).ReturnsAsync(false);
            userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });
            userManagerMock.Setup(x => x.AddClaimsAsync(user, It.IsAny<IEnumerable<Claim>>())).ReturnsAsync(IdentityResult.Success);

            // Mock для IOptions<IdentityOptions>
            var optionsMock = new Mock<IOptions<IdentityOptions>>();
            optionsMock.Setup(x => x.Value).Returns(new IdentityOptions());

            // Mock для UserClaimsPrincipalFactory<User>
            var userClaimsPrincipalFactoryMock = new Mock<UserClaimsPrincipalFactory<User>>(
                userManagerMock.Object,
                optionsMock.Object);

            // Mock для SignInManager<User>
            var signInManagerMock = new Mock<SignInManager<User>>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                userClaimsPrincipalFactoryMock.Object, // Используем mock для UserClaimsPrincipalFactory
                null, null, null, null);

            signInManagerMock.Setup(x => x.PasswordSignInAsync(user, "Test123!", false, false)).ReturnsAsync(SignInResult.Success);

            var mapper = new Mock<IMapper>();

            var authService = new AuthService(mapper.Object, userManagerMock.Object, signInManagerMock.Object);

            //act
            var result = await authService.LoginAsync(new LoginUserResource { Email = "UTest@examle.com", Password = "Test123!" });

            //assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task LoginAsync_ReturnFailed_InvalidPass()
        {
            //arrange
            var user = new User { Email = "UTest@examle.com" };
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync("UTest@examle.com")).ReturnsAsync(user);
            userManagerMock.Setup(x => x.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            userManagerMock.Setup(x => x.IsLockedOutAsync(user)).ReturnsAsync(false);
            userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });
            userManagerMock.Setup(x => x.AddClaimsAsync(user, It.IsAny<IEnumerable<Claim>>())).ReturnsAsync(IdentityResult.Success);

            //Mock для IOptions<IdentityOptions>
            var optionsMock = new Mock<IOptions< IdentityOptions >> ();
            optionsMock.Setup(x => x.Value).Returns(new IdentityOptions());

            //mock для UserClaimsPrincipalFactory<User>
            var userClaimsPrincipalFactoryMock = new Mock<UserClaimsPrincipalFactory<User>>(
                userManagerMock.Object, optionsMock.Object);  

            var signInManagerMock = new Mock<SignInManager<User>>(
            userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            userClaimsPrincipalFactoryMock.Object,
            null, null, null, null);

            signInManagerMock.Setup(x => x.PasswordSignInAsync(user, "wrongpassword", false, false)).ReturnsAsync(SignInResult.Failed);

            var mapper = new Mock<IMapper>();

            var authService = new AuthService(mapper.Object, userManagerMock.Object, signInManagerMock.Object);

            //act
            var result = await authService.LoginAsync(new LoginUserResource { Email = "UTest@examle.com", Password = "wrongpassword" });

            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task RegisterAsync_ReturnSuccess()
        {
            //arrange
            var user = new User { Email = "UTest@examle.com" };
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mapper = new Mock<IMapper>();

            var authService = new AuthService(mapper.Object, userManagerMock.Object, null);

            //act
            var result = await authService
                .RegisterUserAsync(new Resources.UsersResources.CreateUserResource 
                    { Name = "UTest", Email = "UTest@examle.com", Password = "Test123!", ConfirmPassword = "Test123!" });


            //asssert
            Assert.True(result.Succeeded);
        }
    }
}
