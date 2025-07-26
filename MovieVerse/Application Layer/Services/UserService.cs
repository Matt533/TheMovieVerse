using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieVerse.Application_Layer.DTOs.LoginDto;
using MovieVerse.Application_Layer.DTOs.UserDto;
using MovieVerse.Application_Layer.DTOs.UserDtos;
using MovieVerse.Domain_Layer.Exceptions;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.Models;
using MovieVerse.Infrastructional_Layer.Mappers;


namespace MovieVerse.Application_Layer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        
        public UserService(UserManager<User> userManager, 
            ITokenService tokenService, 
            SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            _signInManager = signInManager;
           
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = users.Select(u => u.FromUserToDto()).ToList();

            return dtos;
        }
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return null;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return null;
            }

            return user.FromUserToDto();
        }
        public async Task<AuthResponseDto?> RegisterUserAsync(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                throw new PasswordDontMatchException("Passwords doesn't match!");
            }

            var existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUser != null)
            {
                throw new UsernameExistsException("Username already exists!");
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
            if (!createdUser.Succeeded)
            {
                var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
                throw new Exception($"User creation failed: {errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            var token = _tokenService.CreateToken(user);
            var userDto = user.FromUserToDto();

            return new AuthResponseDto
            {
                User = userDto,
                Token = token
            };

        }
        public async Task<AuthResponseDto?> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if(user == null)
            {
                throw new UserNotFoundException("User doesn't exist!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password, false);

            if(!result.Succeeded)
            {
                throw new UserUnauthorizedException("Username or password invalid!");
            }

            var token = _tokenService.CreateToken(user);
            var userDto = user.FromUserToDto();

            return new AuthResponseDto
            {
                User = userDto,
                Token = token
            };
        }
        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID must be provided.", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false; 

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

    }
}
