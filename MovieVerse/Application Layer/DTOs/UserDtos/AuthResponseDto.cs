using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Application_Layer.DTOs.UserDtos
{
    public record AuthResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
