using System.Text.Json.Serialization;
using MovieVerse.Domain_Layer.DTOs;

namespace MovieVerse.Application_Layer.DTOs.ActorDto
{
    public record ActorDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public string ProfilePath { get; set; } = string.Empty;
        public List<MovieDto> Movies { get; set; } = new List<MovieDto>();
    }
}
