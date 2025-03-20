using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserResource createUserResource)
        {
            var result = await _authService.RegisterUserAsync(createUserResource);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserResource userResource)
        {
            var result = await _authService.LoginAsync(userResource);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "Your account is locked. Please try again later." });
            }

            //if (result.IsNotAllowed)
            //{
            //    return Unauthorized(new { Message = "Please confirm your email." });
            //}

            return Unauthorized(new { Message = "Invalid email or password." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }
    }
}
