using System.ComponentModel.DataAnnotations;

namespace MovieVerse.Domain_Layer.DTOs
{
    public record CreateMovieDto
    {
        [Required]
        public string Language { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public double Rating { get; set; }
        [Required]
        public string PosterUrl { get; set; } = string.Empty;
        [Required]
        public string ReleaseDate { get; set; } = string.Empty;
    }
}
