using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Application_Layer.Interfaces.IRepository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(int page);
        Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genreName, int page);
        Task<IEnumerable<Movie>> GetMoviesByCategoryAsync(string category, int page);
        Task<IEnumerable<Movie>> GetMoviesByYearAsync(string year, int page);
        Task<IEnumerable<Movie>> SearchMovieByTitleAsync(string title);
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task<Movie?> CreateMovieAsync(Movie movie);
        Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto updateMovieDto);
        Task<bool> DeleteMovieByIdAsync(int movieId);
        
    }
}
