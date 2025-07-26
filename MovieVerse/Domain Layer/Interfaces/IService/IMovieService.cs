using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Enumeration;

namespace MovieVerse.Application_Layer.Interfaces.IService
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync(int page);
        Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genreName, int page);
        Task<IEnumerable<MovieDto>> GetMoviesByCategoryAsync(MovieCategory movieCategory, int page);
        Task<IEnumerable<MovieDto>> GetMoviesByYearAsync(string year, int page);
        Task<IEnumerable<MovieDto>> SearchMovieByTitleAsync(string title);
        Task<MovieDto?> GetMovieByIdAsync(int movieId);
        Task<MovieDto?> CreateMovieAsync(CreateMovieDto createMovieDto);
        Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto updateMovieDto);
        Task<bool> DeleteMovieByIdAsync(int movieId);
    }
}
