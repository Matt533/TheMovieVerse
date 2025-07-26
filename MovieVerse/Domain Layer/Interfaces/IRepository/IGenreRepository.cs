using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IRepository
{
    public interface IGenreRepository
    {
        public Task<IEnumerable<Genre>> GetAllGenresAsync();
        public Task<Genre?> GetGenreByNameAsync(string genreName);
    }
}
