using System.Text.Json.Serialization;
using MovieVerse.Application_Layer.DTOs;

namespace MovieVerse.Domain_Layer.Models
{
    public class TmdbMovieResponse
    {
        [JsonPropertyName("results")]
        public List<TMDBMovieDto> Results { get; set; } = new List<TMDBMovieDto>();
    }
}
