using MovieVerse.Application_Layer.DTOs.GenreDto;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface IGenreService
    {
        public Task<IEnumerable<GenreDto>> GetAllGenresAsync();
        public Task<GenreDto?> GetGenreByNameAsync(string genreName);
    }
}
