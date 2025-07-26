using Microsoft.EntityFrameworkCore;
using MovieVerse.Application_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ITMDbService _tmdbService;

        private static int pageSize = 20;
        public MovieRepository(AppDbContext appDbContext, ITMDbService tmdbService)
        {
            this._appDbContext = appDbContext;
            this._tmdbService = tmdbService;
        }
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(int page)
        {
            var movies = await _appDbContext.Movies
                .OrderBy(m => m.Id)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            Console.WriteLine(movies);
            return movies;
        }
        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genreName, int page)
        {
            return await _appDbContext.Movies
                .Where(m => m.MovieGenres.Any(mg => mg.Genre != null && mg.Genre.Name == genreName))
                .OrderBy(m => m.Id)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByCategoryAsync(string category, int page)
        {
            var movies = await _tmdbService.FetchMoviesByCategoryAsync(category, page);
            var filteredMovies = movies.OrderBy(m => m.Id)
                .ToList();

            return filteredMovies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByYearAsync(string year, int page)
        {
            var movies = await _appDbContext.Movies
                .Where(m => m.ReleaseDate.Substring(0,4) == year)
                .OrderBy(m => m.Id)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return movies;
        }
        public async Task<IEnumerable<Movie>> SearchMovieByTitleAsync(string title)
        {
            return await _appDbContext.Movies
                .Where(m => m.Title.Contains(title))
                .OrderBy(m => m.Id)
                .Include(mg=> mg.MovieGenres)
                .ThenInclude(m => m.Genre)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            return await _appDbContext.Movies
                .OrderBy(m => m.Id)
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == movieId);
        }

        public async Task<Movie?> CreateMovieAsync(Movie movie)
        {
            await _appDbContext.Movies.AddAsync(movie);
            await _appDbContext.SaveChangesAsync();

            return movie;
        }

        public async Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto updateMovieDto)
        {
           var movie = await GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            movie.Title = updateMovieDto.Title;
            movie.Description = updateMovieDto.Description;
            movie.ReleaseDate = updateMovieDto.ReleaseDate;
            movie.Rating = updateMovieDto.Rating;
            movie.PosterUrl = updateMovieDto.PosterUrl;
            movie.Language = updateMovieDto.Language;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
       
        public async Task<bool> DeleteMovieByIdAsync(int movieId)
        {
            var movie = await GetMovieByIdAsync(movieId);

            if(movie == null)
            {
                return false;
            }

            _appDbContext.Movies.Remove(movie);

            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
