using MovieVerse.Application_Layer.DTOs.LoginDto;
using MovieVerse.Application_Layer.DTOs.UserDto;
using MovieVerse.Application_Layer.DTOs.UserDtos;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface IUserService
    {
        public Task<AuthResponseDto?> RegisterUserAsync(RegisterDto registerDto);
        public Task<AuthResponseDto?> LoginUserAsync(LoginDto loginDto);
        public Task LogoutUserAsync();
        public Task<IEnumerable<UserDto>> GetAllUsersAsync();
        public Task<UserDto?> GetUserByUsernameAsync(string userName);
        public Task<bool> DeleteUserAsync(string userId);
    }
}
