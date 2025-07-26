using Microsoft.IdentityModel.Tokens;
using MovieVerse.Application_Layer.DTOs.GenreDto;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Infrastructional_Layer.Mappers;

namespace MovieVerse.Application_Layer.Services
{
    public class GenreService : IGenreService
    {
       private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            this._genreRepository = genreRepository;
        }
        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _genreRepository.GetAllGenresAsync();
            var dtos = genres.Select(g => g.FromGenreToDto()).ToList();

            return dtos;
        }

        public async Task<GenreDto?> GetGenreByNameAsync(string genreName)
        {
            if(genreName.IsNullOrEmpty())
            {
                return null;
            }

            var genre = await _genreRepository.GetGenreByNameAsync(genreName);

            if(genre == null)
            {
                return null;
            }

            return genre.FromGenreToDto();
        }
    }
}
