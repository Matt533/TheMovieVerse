using MovieVerse.Domain_Layer.DTOs;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface IUserMovieWatchService
    {
        public Task<IEnumerable<MovieDto>> GetWatchListForUserAsync(string userId, int page);
        public Task<bool> AddMovieToWatchedAsync(string userId, int movieId);
        public Task<bool> RemoveMovieFromWatchedAsync(string userId, int movieId);
        public Task<bool> IsMovieInWatchedAsync(string userId, int movieId);
    }
}
