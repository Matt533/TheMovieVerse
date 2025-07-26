using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieVerse.Application_Layer.DTOs.LoginDto;
using MovieVerse.Application_Layer.DTOs.UserDto;
using MovieVerse.Application_Layer.DTOs.UserDtos;
using MovieVerse.Domain_Layer.Interfaces.IService;

namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterUserAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.RegisterUserAsync(registerDto);
                return Ok(user);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<ActionResult<AuthResponseDto>> LoginUserAsync([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.LoginUserAsync(loginDto);
                return Ok(user);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("/logout")]
        public async Task<IActionResult> LogoutUserAsync()
        {
            await _userService.LogoutUserAsync();

            return Ok("User successfully signed out!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/getAllUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/getByUsername")]
        public async Task<ActionResult<UserDto>> GetByUsernameAsync([FromQuery] string userName)
        {
            try
            {
                var user = await _userService.GetUserByUsernameAsync(userName);
                return Ok(user);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/removeUser/{userId}")]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(userId);
                if (!success)
                    return NotFound($"User with ID {userId} not found.");

                return Ok($"User with ID {userId} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
        
}
