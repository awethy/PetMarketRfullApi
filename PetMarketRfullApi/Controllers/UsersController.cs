using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Resources.AccountResources;
using PetMarketRfullApi.Resources.UsersResources;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        } 

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateUserPolicy")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserResource updateUser)
        {
            var result = await _userService.UpdateUserAsync(id, updateUser);
            if (result.Succeeded)
            {
                return Ok(new {message = "User updated successfully"});
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminOnly")]
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
