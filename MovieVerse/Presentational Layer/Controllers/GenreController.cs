using Microsoft.AspNetCore.Mvc;
using MovieVerse.Application_Layer.DTOs.GenreDto;
using MovieVerse.Domain_Layer.Interfaces.IService;


namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            this._genreService = genreService;
        }

        [HttpGet("/getAllGenres")]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllAsync()
        {
            try
            {
                var genres = await _genreService.GetAllGenresAsync();
                return Ok(genres);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/getGenreByName")]
        public async Task<ActionResult<GenreDto>> GetByNameAsync([FromQuery] string genreName)
        {
            try
            {
                var genre = await _genreService.GetGenreByNameAsync(genreName);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
