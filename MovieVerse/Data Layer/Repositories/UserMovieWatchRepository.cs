using Microsoft.EntityFrameworkCore;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer.Repositories
{
    public class UserMovieWatchRepository : IUserMovieWatchRepository
    {
        private readonly AppDbContext _appDbContext;
        private static int pageSize = 20;
        public UserMovieWatchRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Movie>> GetWatchListForUserAsync(string userId, int page)
        {
            var movies = await _appDbContext.MoviesWatch
                .Where(mw => mw.UserId.Equals(userId))
                .OrderBy(mw => mw.MovieId)
                .Include(mw => mw.Movie)
                .Skip(((page - 1) * pageSize))
                .Take(pageSize)
                .Select(mw => mw.Movie)
                .ToListAsync();

            return movies;
        }
        public async Task<bool> AddMovieToWatchedAsync(string userId, int movieId)
        {
            var exists = await _appDbContext.MoviesWatch
                .AnyAsync(umw => umw.UserId == userId && umw.MovieId == movieId);

            if (exists)
                return false;

            var watched = new UserMovieWatch
            {
                UserId = userId,
                MovieId = movieId,
                WatchedOn = DateTime.Now
            };

            await _appDbContext.MoviesWatch.AddAsync(watched);
            await _appDbContext.SaveChangesAsync();
            return true;

        }
        public async Task<bool> RemoveMovieFromWatchedAsync(string userId, int movieId)
        {
            var watched = await _appDbContext.MoviesWatch.FirstOrDefaultAsync(umw => umw.UserId == userId && umw.MovieId == movieId);

            if(watched == null)
            {
                return false;
            }

            _appDbContext.MoviesWatch.Remove(watched);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsMovieInWatchedAsync(string userId, int movieId)
        {
            return await _appDbContext.MoviesWatch.AnyAsync(umw => umw.UserId == userId && umw.MovieId == umw.MovieId);
        }
    }
}
