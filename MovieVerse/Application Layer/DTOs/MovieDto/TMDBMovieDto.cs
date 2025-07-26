using System.Text.Json.Serialization;
namespace MovieVerse.Application_Layer.DTOs
{
    public record TMDBMovieDto
    {
        [JsonPropertyName("id")]
        public int TmdbId { get; set; }
        [JsonPropertyName ("original_language")]
        public string Language { get; set; } = string.Empty;
        [JsonPropertyName ("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName ("overview")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName ("vote_average")]
        public double Rating { get; set; }
        [JsonPropertyName ("poster_path")]
        public string PosterUrl { get; set; } = string.Empty;
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = string.Empty;

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
