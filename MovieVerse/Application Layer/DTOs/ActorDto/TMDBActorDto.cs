using MovieVerse.Domain_Layer.DTOs;
using System.Text.Json.Serialization;

namespace MovieVerse.Application_Layer.DTOs.ActorDto
{
    public record TMDBActorDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("biography")]
        public string Biography { get; set; } = string.Empty;
        [JsonPropertyName("birthday")]
        public string Birthday { get; set; } = string.Empty;
        [JsonPropertyName("profile_path")]
        public string ProfilePath { get; set; } = string.Empty;
        public List<MovieDto> Movies { get; set; } = new List<MovieDto>();
    }
}
