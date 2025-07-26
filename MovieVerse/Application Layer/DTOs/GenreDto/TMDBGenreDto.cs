using System.Text.Json.Serialization;

namespace MovieVerse.Application_Layer.DTOs.GenreDto
{
    public record TMDBGenreDto
    {
        [JsonPropertyName("id")]
        public int TmdbId { get; set; }
        [JsonPropertyName ("name")]
        public string Name { get; set; } = string.Empty;
    }
}
