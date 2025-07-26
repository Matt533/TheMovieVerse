using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Interfaces.IService;

namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMovieWatchController : ControllerBase
    {
        private readonly IUserMovieWatchService _userMovieWatchService;
        public UserMovieWatchController(IUserMovieWatchService userMovieWatchService)
        {
            this._userMovieWatchService = userMovieWatchService;
        }

        [Authorize]
        [HttpGet("/getWatchList")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetWatchListByUserAsync([FromQuery] int page)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var movies = await _userMovieWatchService.GetWatchListForUserAsync(userId, page);
                
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("/addToWatchList/{movieId:int}")]
        public async Task<ActionResult<bool>> AddToWatchListAsync([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userMovieWatchService.AddMovieToWatchedAsync(userId, movieId);

                if (info)
                    return Ok(true);

                return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("/removeFromWatchList/{movieId:int}")]
        public async Task<ActionResult<bool>> RemoveFromWatchListAsync([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userMovieWatchService.RemoveMovieFromWatchedAsync(userId, movieId);
                if (info) 
                    return Ok(true);

                 return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("/isInWatchList/{movieId:int}")]
        public async Task<ActionResult<bool>> IsInWatchListAsync([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userMovieWatchService.IsMovieInWatchedAsync(userId, movieId);
                if(info)
                    return Ok(true);

                return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
