using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Domain_Layer.Interfaces.IService;

namespace MovieVerse.Presentational_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService actorService)
        {
            this._actorService = actorService;
        }

        [HttpGet("/getAllActors")]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetAllAsync([FromQuery] int page)
        {
            try
            {
                var actors = await _actorService.GetAllActorsAsync(page);

                return Ok(actors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("/getAllOverviews")]
        public async Task<ActionResult<IEnumerable<ActorOverviewDto>>> GetAllOverviewsAsync([FromQuery] int page)
        {
            try
            {
                var overviews = await _actorService.GetAllActorsOverviewAsync(page);
                return Ok(overviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/getActorById/{actorId:int}")]
        public async Task<ActionResult<ActorDto>> GetByIdAsync([FromRoute] int actorId)
        {
            try
            {
                var actor = await _actorService.GetActorByIdAsync(actorId);
                return Ok(actor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/createActor")]
        public async Task<ActionResult<ActorDto>> CreateAsync([FromBody] CreateActorDto createActorDto)
        {
            try
            {
                var actor = await _actorService.CreateActorAsync(createActorDto);
                return Ok(actor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("/updateActor/{actorId:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int actorId, [FromBody] UpdateActorDto updateActorDto)
        {
            try
            {
                var info = await _actorService.UpdateActorAsync(actorId, updateActorDto);
                if (!info)
                    return NotFound();

                return NoContent();
                
            }
            catch(Exception ex)
            { 
                return BadRequest(ex); 
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/deleteActor/{actorId:int}")]
        public async Task<IActionResult> DeleteActorAsync([FromRoute] int actorId)
        {
            try
            {
                var info = await _actorService.DeleteActorAsync(actorId);
                if (!info)
                    return NotFound();

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
