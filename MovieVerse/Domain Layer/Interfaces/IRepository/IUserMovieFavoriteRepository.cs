using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IRepository
{
    public interface IUserMovieFavoriteRepository
    {
        public Task<IEnumerable<Movie>> GetFavoritesListForUserAsync(string userId, int page);
        public Task<bool> AddMovieToFavoritesAsync(string userId, int movieId);

        public Task<bool> RemoveMovieFromFavoritesAsync(string userId, int movieId);

        public Task<bool> IsMovieInFavoritesAsync(string userId, int movieId);
    }
}
