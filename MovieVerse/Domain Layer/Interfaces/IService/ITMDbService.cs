using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface ITMDbService
    {
        Task SyncMovies(int totalPages);
        Task SyncGenres();
        Task SyncActors(int totalPages);
        Task<IEnumerable<Movie>> FetchAndSaveMoviesAsync(string category, int page);
        Task<IEnumerable<Movie>> FetchMoviesByCategoryAsync(string category, int page);
        Task<IEnumerable<Genre>> FetchAndSaveGenresAsync();
        Task<Actor?> FetchAndSaveActorAsync(int page);
        Task<IEnumerable<Movie>> FetchAndSaveMoviesForActorsAsync(int actorId);
        Task<IEnumerable<ActorsOverview>> FetchAndSaveActorsOverviewAsync(int page);
    }
}
