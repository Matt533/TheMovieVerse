using Microsoft.EntityFrameworkCore;
using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly AppDbContext _appDbContext;
        private static int pageSize = 20;
        public ActorRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Actor>> GetAllActorsAsync(int page)
        {
            var actors = await _appDbContext.Actors
                .OrderBy(a => a.Id)
                .Include(a => a.PlayedInMovies)
                .ThenInclude(m => m.Movie)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return actors;
        }
        public async Task<IEnumerable<ActorsOverview>> GetAllActorsOverviewsAsync(int page)
        {
            var actorsOverview = await _appDbContext.ActorsOverview
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return actorsOverview;
        }

        public async Task<Actor?> GetActorByIdAsync(int actorId)
        {
            var actor = await _appDbContext.Actors.FirstOrDefaultAsync(a => a.Id == actorId);
            return actor;
        }
        public async Task<Actor?> CreateActorAsync(Actor actor)
        {
            await _appDbContext.Actors.AddAsync(actor);
            await _appDbContext.SaveChangesAsync();

            return actor;
        }
        public async Task<bool> UpdateActorAsync(int actorId, UpdateActorDto updateActorDto)
        {
            var actor = await GetActorByIdAsync(actorId);

            if(actor == null)
            {
                return false;
            }

            actor.Name = updateActorDto.Name;
            actor.Birthday = updateActorDto.Birthday;
            actor.Biography = updateActorDto.Biography;
            actor.ProfilePath = updateActorDto.ProfilePath;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteActorAsync(int actorId)
        {
            var actor = await GetActorByIdAsync(actorId);

            if(actor == null)
            {
                return false;
            }

            _appDbContext.Actors.Remove(actor);
            await _appDbContext.SaveChangesAsync();
            return true; 
        }
    }
}
