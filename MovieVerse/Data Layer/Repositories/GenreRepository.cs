using Microsoft.EntityFrameworkCore;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _appDbContext;
        public GenreRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _appDbContext.Genres.ToListAsync();
        }

        public async Task<Genre?> GetGenreByNameAsync(string genreName)
        {
            return await _appDbContext.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
        }
    }
}
