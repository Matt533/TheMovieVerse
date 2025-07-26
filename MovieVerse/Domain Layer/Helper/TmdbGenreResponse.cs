using System.Text.Json.Serialization;
using MovieVerse.Application_Layer.DTOs.GenreDto;

namespace MovieVerse.Domain_Layer.Helper
{
    public class TmdbGenreResponse
    {
        [JsonPropertyName("genres")]
        public List<TMDBGenreDto> Genres { get; set; } = new List<TMDBGenreDto>();
    }
}
