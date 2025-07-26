using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieVerse.Application_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Enumeration;

namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            this._movieService = movieService;
        }

        [HttpGet("/getAllMovies")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllAsync([FromQuery] int page)
        {
            try
            {
                var movies = await _movieService.GetAllMoviesAsync(page);
             
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/getMoviesByGenre")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetByGenreAsync([FromQuery] string genreName, [FromQuery] int page)
        {
            try
            {
                var movies = await _movieService.GetMoviesByGenreAsync(genreName, page);

                return Ok(movies);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/getMoviesByCategory")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetByCategoryAsync([FromQuery] MovieCategory movieCategory, [FromQuery] int page)
        {
            try
            {
                var movies = await _movieService.GetMoviesByCategoryAsync(movieCategory, page);
                
                return Ok(movies);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/getMoviesByYear")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetByYearAsync([FromQuery] string year, [FromQuery] int page)
        {
            try
            {
                var movies = await _movieService.GetMoviesByYearAsync(year, page);

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/searchMoviesByTitle")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> SearchMoviesAsync([FromQuery] string title)
        {
            try
            {
                var movies = await _movieService.SearchMovieByTitleAsync(title);

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/getMovieById/{movieId:int}", Name = "GetMovieById")]
        public async Task<ActionResult<MovieDto>> GetByIdAsync([FromRoute] int movieId)
        {
            try
            {
                var movie = await _movieService.GetMovieByIdAsync(movieId);

                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/createMovie")]
        public async Task<ActionResult<MovieDto>> CreateAsync([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input is invalid");
            }
            try
            {
                var createdMovie = await _movieService.CreateMovieAsync(createMovieDto);

                return CreatedAtRoute("GetMovieById", new { movieId = createdMovie.Id }, createdMovie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updateMovie/{movieId:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int movieId, [FromBody] UpdateMovieDto updateMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input is invalid");
            }
            try
            {
                var updatedMovie = await _movieService.UpdateMovieAsync(movieId, updateMovieDto);

                if(!updatedMovie)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/deleteMovie/{movieId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int movieId)
        {
            try
            {
                var deletedMovie = await _movieService.DeleteMovieByIdAsync(movieId);
                if(!deletedMovie)
                    return NotFound();

                return NoContent();
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
