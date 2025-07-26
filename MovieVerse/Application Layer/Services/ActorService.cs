using Microsoft.EntityFrameworkCore;
using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Data_Layer;
using MovieVerse.Data_Layer.Repositories;
using MovieVerse.Domain_Layer.Exceptions;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.Models;
using MovieVerse.Infrastructional_Layer.Mappers;

namespace MovieVerse.Application_Layer.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        public ActorService(IActorRepository actorRepository)
        {
            this._actorRepository = actorRepository;
        }
        public async Task<IEnumerable<ActorOverviewDto>> GetAllActorsOverviewAsync(int page)
        {
           if(page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

           var actorsOverview = await _actorRepository.GetAllActorsOverviewsAsync(page);

           var actorOverviewDtos = actorsOverview.Select(ao => ao.FromActorOverviewToActorOverviewDto());
           
           return actorOverviewDtos;
        }
        public async Task<IEnumerable<ActorDto>> GetAllActorsAsync(int page)
        {
            if( page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }
            var actors = await _actorRepository.GetAllActorsAsync(page);
            
            var actorsDtos = actors.Select(a => a.FromActorToActorDto());

            return actorsDtos;
        }
        public async Task<ActorDto?> GetActorByIdAsync(int actorId)
        {
            var actor = await _actorRepository.GetActorByIdAsync(actorId);

            if (actor == null)
            {
                throw new ActorNotFoundException("Actor doesn't exist!");
            }

            return actor.FromActorToActorDto();
        }

        public async Task<ActorDto?> CreateActorAsync(CreateActorDto createActorDto)
        {
            var actor = createActorDto.FromActorDtoToModel();

            var createdActor = await _actorRepository.CreateActorAsync(actor);
           
            if(createdActor == null)
            {
                throw new Exception("Actor creation error!");
            }

            return createdActor.FromActorToActorDto();
        }
        public async Task<bool> UpdateActorAsync(int actorId, UpdateActorDto updateActorDto)
        {
           var info = await _actorRepository.UpdateActorAsync(actorId, updateActorDto);
            
            if(info == false)
            {
                throw new Exception("Updating actor failed!");
            }

            return true;
        }

        public async Task<bool> DeleteActorAsync(int actorId)
        {
           var info = await _actorRepository.DeleteActorAsync(actorId);

            if(info == false)
            {
                throw new Exception("Failed to delete actor!");
            }

            return true;
        }
    }
}
