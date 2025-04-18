﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Resources.AccountsResources;
using PetMarketRfullApi.Application.Resources.UsersResources;

namespace PetMarketRfullApi.Web.Controllers
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

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserResource userResource)
        {
            var result = await _authService.LoginAsync(userResource);

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }
    }
}
