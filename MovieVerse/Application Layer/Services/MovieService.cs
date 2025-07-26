
using Microsoft.IdentityModel.Tokens;
using MovieVerse.Application_Layer.Interfaces.IRepository;
using MovieVerse.Application_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Enumeration;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Infrastructional_Layer.Exceptions;
using MovieVerse.Infrastructional_Layer.Mappers;

namespace MovieVerse.Application_Layer.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            this._movieRepository = movieRepository;
            this._genreRepository = genreRepository;    
        }
        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync(int page)
        {
            if(page <= 0)
            {
                throw new ArgumentException("Invalid input!");
            }

            var movies = await _movieRepository.GetAllMoviesAsync(page);
            var dtos = movies.Select(m => m.FromMovieToMovieDTO()).ToList();

            return dtos;
        }
        public async Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genreName, int page)
        {
            var genre = await _genreRepository.GetGenreByNameAsync(genreName);

            if(genre == null || page <= 0)
            {
                throw new ArgumentException("Invalid input!");
            }

            var movies = await _movieRepository.GetMoviesByGenreAsync(genreName, page);
            var dtos = movies.Select(m => m.FromMovieToMovieDTOForGenres(genreName)).ToList();
            return dtos;
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesByCategoryAsync(MovieCategory movieCategory, int page)
        {
            var category = movieCategory.FromEnumToString();

            if (category.IsNullOrEmpty() || page <= 0)
            {
                throw new ArgumentException("Invalid input!");
            }

            var movies = await _movieRepository.GetMoviesByCategoryAsync(category, page);

            var dtos = movies.Select(m => m.FromMovieToMovieDTO()).ToList();

            return dtos;
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesByYearAsync(string year, int page)
        {
            if(year.IsNullOrEmpty() || page <= 0)
            {
                throw new ArgumentException("Invalid input!");
            }

            var movies = await _movieRepository.GetMoviesByYearAsync(year, page);
            var dtos = movies.Select(m => m.FromMovieToMovieDTO());
            return dtos;
        }

        public async Task<IEnumerable<MovieDto>> SearchMovieByTitleAsync(string title)
        {
            if (title.IsNullOrEmpty())
            {
                throw new ArgumentException("Invalid input!");
            }

            var movies = await _movieRepository.SearchMovieByTitleAsync(title);
            var dtos = movies.Select(m => m.FromMovieToMovieDTO()).ToList();
            return dtos;
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);

            if(movie == null)
            {
                throw new MovieNotFoundException("Movie id doesn't exist!");
            }

            return movie.FromMovieToMovieDTO();
        }

        public async Task<MovieDto?> CreateMovieAsync(CreateMovieDto createMovieDto)
        {
            var movie = createMovieDto.FromMovieDtoToModel();

            await _movieRepository.CreateMovieAsync(movie);

            return movie.FromMovieToMovieDTO();
        }
        public async Task<bool> UpdateMovieAsync(int movieId, UpdateMovieDto updateMovieDto)
        {
            var movieInfo = await _movieRepository.UpdateMovieAsync(movieId, updateMovieDto);
            
            if(!movieInfo)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteMovieByIdAsync(int movieId)
        {
            var movieInfo = await _movieRepository.DeleteMovieByIdAsync(movieId);

            if(!movieInfo)
            {
                return false;
            }
            return true;
        }
    }
}
