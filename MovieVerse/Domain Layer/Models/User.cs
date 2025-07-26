using Microsoft.AspNetCore.Identity;

namespace MovieVerse.Domain_Layer.Models
{
    public class User : IdentityUser
    {
        public List<UserMovieWatch> WatchedMovies { get; set; } = new List<UserMovieWatch>();
        public List<UserMovieFavorite> FavoriteMovies { get; set; } = new List<UserMovieFavorite>();
    }
}
