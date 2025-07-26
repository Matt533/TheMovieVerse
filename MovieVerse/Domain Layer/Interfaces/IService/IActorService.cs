using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface IActorService
    {
        public Task<IEnumerable<ActorDto>> GetAllActorsAsync(int page);
        public Task<IEnumerable<ActorOverviewDto>> GetAllActorsOverviewAsync(int page);
        public Task<ActorDto?> GetActorByIdAsync(int actorId);
        public Task<ActorDto?> CreateActorAsync(CreateActorDto createActorDto);
        public Task<bool> UpdateActorAsync(int actorId, UpdateActorDto updateActorDto);
        public Task<bool> DeleteActorAsync(int actorId);
    }
}
