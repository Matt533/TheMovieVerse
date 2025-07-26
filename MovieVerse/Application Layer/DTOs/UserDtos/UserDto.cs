using System.ComponentModel.DataAnnotations;

namespace MovieVerse.Application_Layer.DTOs.UserDtos
{
    public record UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
