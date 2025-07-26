using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IRepository
{
    public interface IUserMovieWatchRepository
    {
        public Task<IEnumerable<Movie>> GetWatchListForUserAsync(string userId, int page);
        public Task<bool> AddMovieToWatchedAsync(string userId, int movieId);
        public Task<bool> RemoveMovieFromWatchedAsync(string userId, int movieId);
        public Task<bool> IsMovieInWatchedAsync(string userId, int movieId);
    }
}
