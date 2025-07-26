using System.ComponentModel.DataAnnotations;

namespace MovieVerse.Application_Layer.DTOs.LoginDto
{
    public record LoginDto
    {

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }

}
