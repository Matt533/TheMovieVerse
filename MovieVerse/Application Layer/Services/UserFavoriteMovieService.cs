using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieVerse.Application_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.Models;
using MovieVerse.Infrastructional_Layer.Mappers;

namespace MovieVerse.Application_Layer.Services
{
    public class UserFavoriteMovieService : IUserMovieFavoriteService
    {
        private readonly IUserMovieFavoriteRepository _userMovieFavoriteRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly UserManager<User> _userManager;
        public UserFavoriteMovieService(IUserMovieFavoriteRepository userMovieFavoriteRepository, 
                                    UserManager<User> userManager,
                                     IMovieRepository movieRepository)
        {
            this._userMovieFavoriteRepository = userMovieFavoriteRepository;
            this._movieRepository = movieRepository;
            this._userManager = userManager;
        }
        public async Task<IEnumerable<MovieDto>> GetFavoritesListForUserAsync(string userId, int page)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                return Enumerable.Empty<MovieDto>();
            }

            var favoriteMovies = await _userMovieFavoriteRepository.GetFavoritesListForUserAsync(userId, page);
            var dtos = favoriteMovies.Select(fm => fm.FromMovieToMovieDTO()).ToList();

            return dtos;
        }
        public async Task<bool> AddMovieToFavoritesAsync(string userId, int movieId)
        {

            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;   
            }

            return await _userMovieFavoriteRepository.AddMovieToFavoritesAsync(userId, movieId);
        }
        public async Task<bool> RemoveMovieFromFavoritesAsync(string userId, int movieId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            return await _userMovieFavoriteRepository.RemoveMovieFromFavoritesAsync(userId, movieId);
        }

        public async Task<bool> IsMovieInFavoritesAsync(string userId, int movieId)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            return await _userMovieFavoriteRepository.IsMovieInFavoritesAsync(userId, movieId);
        }
    }
}
