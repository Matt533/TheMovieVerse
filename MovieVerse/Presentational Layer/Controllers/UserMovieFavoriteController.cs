using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Interfaces.IService;

namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMovieFavoriteController : ControllerBase
    {
        private readonly IUserMovieFavoriteService _userFavoriteService;
        public UserMovieFavoriteController(IUserMovieFavoriteService userFavoriteService)
        {
            this._userFavoriteService = userFavoriteService;
        }

        [Authorize]
        [HttpGet("/getFavoriteList")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetFavoritesAsync([FromQuery] int page)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var favoriteList = await _userFavoriteService.GetFavoritesListForUserAsync(userId, page);
                
                return Ok(favoriteList);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("/addToFavorites/{movieId:int}")]

        public async Task<ActionResult<bool>> AddToFavoritesAsync([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userFavoriteService.AddMovieToFavoritesAsync(userId, movieId);

                if(info)
                    return Ok(true);

                return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("/removeFromFavorites/{movieId:int}")]
        public async Task<ActionResult<bool>> RemoveFromFavoritesAsync([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userFavoriteService.RemoveMovieFromFavoritesAsync(userId, movieId);

                if(info)
                    return Ok(true);

                return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("/isInFavorites/{movieId:int}")]
        public async Task<ActionResult<bool>> IsInFavoriteList([FromRoute] int movieId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized("User id not found in token!");
                }

                var info = await _userFavoriteService.IsMovieInFavoritesAsync(userId, movieId);
                if (info)
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
