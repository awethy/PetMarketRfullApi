using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources;
using System.Collections.Specialized;

namespace PetMarketRfullApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResource>>> GetUsers()
        {
            var users =  await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("string")]
        public async Task<ActionResult<UserResource>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult/*ActionResult<UserResource>*/> Register(CreateUserResource createUserResource)
        {
            var result = await _userService.RegisterUserAsync(createUserResource);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var user = await _userService.RegisterUserAsync(createUserResource);
            //return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserResource userResource)
        {
            var result = await _userService.LoginAsync(userResource);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "Your account is locked. Please try again later." });
            }

            if (result.IsNotAllowed)
            {
                return Unauthorized(new { Message = "Please confirm your email." });
            }

            return Unauthorized(new { Message = "Invalid email or password." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return Ok();
        }

        [HttpPut("string")]
        public async Task<ActionResult<UserResource>> UpdateUser(string id, UpdateUserResource updateUser)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                await _userService.UpdateUserAsync(id, updateUser);
                return Ok(updateUser);
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("string")]
        public async Task<ActionResult<UserResource>> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}
