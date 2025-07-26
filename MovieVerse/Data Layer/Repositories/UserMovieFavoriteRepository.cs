using Microsoft.EntityFrameworkCore;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer.Repositories
{
    public class UserMovieFavoriteRepository : IUserMovieFavoriteRepository
    {
        private readonly AppDbContext _appDbContext;
        private static int pageSize = 20;
        public UserMovieFavoriteRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Movie>> GetFavoritesListForUserAsync(string userId, int page)
        {
            var movies = await _appDbContext.MoviesFavorite
                .Where(mw => mw.UserId == userId)
                .OrderBy(mw => mw.MovieId)
                .Include(mw => mw.Movie)
                .Skip(((page - 1) * pageSize))
                .Take(pageSize)
                .Select(mw => mw.Movie)
                .ToListAsync();

            return movies;
        }
        public async Task<bool> AddMovieToFavoritesAsync(string userId, int movieId)
        {
            var favorite = await _appDbContext.MoviesFavorite
                .AnyAsync(mw => mw.UserId == userId && mw.MovieId == movieId);

            if(favorite)
            {
                return false;
            }

            var favorited = new UserMovieFavorite
            {
                UserId = userId,
                MovieId = movieId,
                AddedOn = DateTime.Now
            };

            await _appDbContext.MoviesFavorite.AddAsync(favorited);
            await _appDbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> RemoveMovieFromFavoritesAsync(string userId, int movieId)
        {
            var favorite = await _appDbContext.MoviesFavorite
                 .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);

            if(favorite == null)
                return false;

             _appDbContext.MoviesFavorite.Remove(favorite);
            await _appDbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> IsMovieInFavoritesAsync(string userId, int movieId)
        {
            return await _appDbContext.MoviesFavorite.AnyAsync(f => f.UserId == userId && f.MovieId == movieId);
        }
    }
}
