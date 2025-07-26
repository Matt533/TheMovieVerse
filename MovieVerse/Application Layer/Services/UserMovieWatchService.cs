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
    public class UserMovieWatchService : IUserMovieWatchService
    {
        private readonly IUserMovieWatchRepository _userMovieWatchRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly UserManager<User> _userManager;
        public UserMovieWatchService(IUserMovieWatchRepository userMovieWatchRepository, 
                                    UserManager<User> userManager,
                                    IMovieRepository movieRepository)
        {
            this._userMovieWatchRepository = userMovieWatchRepository;
            this._movieRepository = movieRepository;
            this._userManager = userManager;
        }
        public async Task<IEnumerable<MovieDto>> GetWatchListForUserAsync(string userId, int page)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Enumerable.Empty<MovieDto>();
            }

           var watchedMovies = await _userMovieWatchRepository.GetWatchListForUserAsync(userId, page);
           var dtos = watchedMovies.Select(m => m.FromMovieToMovieDTO()).ToList();

           return dtos;
            
        }
        public async Task<bool> AddMovieToWatchedAsync(string userId, int movieId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

           return await _userMovieWatchRepository.AddMovieToWatchedAsync(userId, movieId);
        }

        public async Task<bool> IsMovieInWatchedAsync(string userId, int movieId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            return await _userMovieWatchRepository.IsMovieInWatchedAsync(userId, movieId);
        }

        public async Task<bool> RemoveMovieFromWatchedAsync(string userId, int movieId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            return await _userMovieWatchRepository.RemoveMovieFromWatchedAsync(userId, movieId);
        }
    }
}
