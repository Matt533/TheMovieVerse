using System.ComponentModel;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface IUserMovieFavoriteService
    {
        public Task<IEnumerable<MovieDto>> GetFavoritesListForUserAsync(string userId, int page);
        public Task<bool> AddMovieToFavoritesAsync(string userId, int movieId);
        public Task<bool> RemoveMovieFromFavoritesAsync(string userId, int movieId);
        public Task<bool> IsMovieInFavoritesAsync(string userId, int movieId);  
    }
}
