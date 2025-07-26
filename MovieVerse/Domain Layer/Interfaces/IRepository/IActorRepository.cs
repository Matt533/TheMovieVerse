using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IRepository
{
    public interface IActorRepository
    {
        public Task<IEnumerable<Actor>> GetAllActorsAsync(int page);
        public Task<IEnumerable<ActorsOverview>> GetAllActorsOverviewsAsync(int page); 
        public Task<Actor?> GetActorByIdAsync(int actorId);
        public Task<Actor?> CreateActorAsync(Actor actor);
        public Task<bool> UpdateActorAsync (int actorId, UpdateActorDto updateActorDto);
        public Task<bool> DeleteActorAsync(int actorId);
    }
}
