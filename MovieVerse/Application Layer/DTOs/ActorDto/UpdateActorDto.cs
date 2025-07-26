using MovieVerse.Domain_Layer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MovieVerse.Application_Layer.DTOs.ActorDto
{
    public record UpdateActorDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Biography { get; set; } = string.Empty;
        [Required]
        public string Birthday { get; set; } = string.Empty;
        [Required]
        public string ProfilePath { get; set; } = string.Empty;
    }
}
