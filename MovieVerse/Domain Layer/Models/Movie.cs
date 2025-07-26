using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieVerse.Domain_Layer.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public List<UserMovieFavorite> FavoritedByUsers { get; set; } = new List<UserMovieFavorite>();
        public List<UserMovieWatch> WatchedByUsers { get; set; } = new List<UserMovieWatch>();
        public List<GenreMovies> MovieGenres { get; set; } = new List<GenreMovies>();
        public List<ActorMovies> ActorsList { get; set; } = new List<ActorMovies>();
    }
}
