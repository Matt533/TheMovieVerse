using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace MovieVerse.Application_Layer.DTOs.UserDto
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword {  get; set; } = string.Empty;    
    }
}
