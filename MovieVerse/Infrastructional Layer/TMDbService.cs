using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Data_Layer;
using MovieVerse.Domain_Layer.Enumeration;
using MovieVerse.Domain_Layer.Helper;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Domain_Layer.Models;
using MovieVerse.Infrastructional_Layer.Mappers;

namespace MovieVerse.Infrastructional_Layer
{
    public class TMDbService : ITMDbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly AppDbContext _appDbContext;
        public TMDbService(IConfiguration configuration, HttpClient httpClient, AppDbContext appDbContext)
        {

            this._httpClient = httpClient;
            this._appDbContext = appDbContext;
            this._apiKey = configuration["TMDBApi:Api_Key"] ?? throw new ArgumentNullException("Api key not found in configuration");
        }
        public async Task SyncMovies(int totalPages)
        {
            foreach (MovieCategory movieCategory in Enum.GetValues(typeof(MovieCategory)))
            {
                for (int page = 1; page <= totalPages; page++)
                {
                    await FetchAndSaveMoviesAsync(movieCategory.FromEnumToString(), page);
                }
            }
        }

        public async Task SyncGenres()
        {
            await FetchAndSaveGenresAsync();
        }

        public async Task SyncActors (int totalPages)
        {
            for(int page = 1; page <= totalPages; page++)
            {
                var actors = await FetchAndSaveActorsOverviewAsync(page);

                foreach (var actor in actors)
                {
                    await FetchAndSaveActorAsync(actor.Id);
                    await FetchAndSaveMoviesForActorsAsync(actor.Id);
                }
            }
        }

        public async Task<IEnumerable<Movie>> FetchAndSaveMoviesAsync(string category, int page)
        {
            var url = $"https://api.themoviedb.org/3/movie/{category}?api_key={_apiKey}&language=en-US&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch movies for category '{category}', page {page}. Status: {response.StatusCode}");
                return Enumerable.Empty<Movie>();
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TmdbMovieResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var movies = tmdbResponse?.Results.Select(tmdb => tmdb.FromTmdbDtoToMovie()).ToList() ?? new List<Movie>();

            var existingIds = await _appDbContext.Movies.Select(m => m.Id).ToListAsync();

            var newMovies = movies.Where(m => !existingIds.Contains(m.Id)).ToList();

            var genres = await _appDbContext.Genres.ToListAsync();
           
            foreach (var movie in newMovies)
            {

                if(movie.MovieGenres == null)
                {
                    continue;
                }
                var movieGenres = movie.MovieGenres?.Where(mg => genres.Any(g => g.Id == mg.GenreId))
                    .Select(mg => new GenreMovies
                    {
                        MovieId = movie.Id,
                        GenreId = mg.GenreId
                    }).Distinct().ToList();

                movie.MovieGenres = movieGenres;

                var savedMovie = movie;

                if (savedMovie.MovieGenres == null)
                {
                    continue;
                }
            }

            if (newMovies.Any())
            {
                try
                {
                    await _appDbContext.AddRangeAsync(newMovies);
                    await _appDbContext.SaveChangesAsync();
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return newMovies;
        }

        public async Task<IEnumerable<Movie>> FetchMoviesByCategoryAsync(string category, int page)
        {
            var url = $"https://api.themoviedb.org/3/movie/{category}?api_key={_apiKey}&language=en-US&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch movies for category '{category}', page {page}. Status: {response.StatusCode}");
                return Enumerable.Empty<Movie>();
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TmdbMovieResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var movies = tmdbResponse?.Results.Select(tmdb => tmdb.FromTmdbDtoToMovie()).ToList() ?? new List<Movie>();

            var existingIds = await _appDbContext.Movies.Select(m => m.Id).ToListAsync();

            var newMovies = movies.Where(m => !existingIds.Contains(m.Id)).ToList();

            if (newMovies.Any())
            {
                try
                {
                    await _appDbContext.AddRangeAsync(newMovies);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            var genres = await _appDbContext.Genres.Select(g => g.Id).ToListAsync();

            foreach (var movie in newMovies)
            {
                var savedMovie = movie;

                if (savedMovie.MovieGenres == null)
                {
                    continue;
                }

            }

            await _appDbContext.SaveChangesAsync();

            return newMovies;
        }

        public async Task<IEnumerable<ActorsOverview>> FetchAndSaveActorsOverviewAsync(int page)
        {
            var url = $"https://api.themoviedb.org/3/person/popular?api_key={_apiKey}&language=en-US&page={page}";

            var response = await _httpClient.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TmdbActorOverviewResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

           var actorsOverview = tmdbResponse?.Results.Select(tmdb => tmdb.FromActorOverviewDtoToActorOverview()).ToList() ?? new List<ActorsOverview>();
            
           var existingIds = await _appDbContext.ActorsOverview.Select(ao => ao.Id).ToListAsync();

           var newActorsOverview = actorsOverview.Where(ao => !existingIds.Contains(ao.Id)).ToList();
            
            if(newActorsOverview.Any())
            {
                try
                {
                    await _appDbContext.AddRangeAsync(newActorsOverview);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            await _appDbContext.SaveChangesAsync();
            return newActorsOverview;
        }

        public async Task<Actor?> FetchAndSaveActorAsync(int actorId)
        {
            var api_url = $"https://api.themoviedb.org/3/person/{actorId}?api_key={_apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(api_url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch actor {actorId}. Status: {response.StatusCode}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TMDBActorDto>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var actors = await _appDbContext.Actors.Include(a => a.PlayedInMovies).ToListAsync();
            
            var actor = new Actor
            {
                Id = tmdbResponse.Id,
                Name = tmdbResponse.Name,
                Biography = tmdbResponse.Biography,
                Birthday = tmdbResponse.Birthday,
                ProfilePath = $"https://image.tmdb.org/t/p/w500{tmdbResponse.ProfilePath}",
                PlayedInMovies = new List<ActorMovies>()
            };

            var existing = actors.FirstOrDefault(a => a.Id == actor.Id);

            if (existing == null)
            {
                await _appDbContext.AddAsync(actor);
            }
            else
            {
                existing.Name = actor.Name;
                existing.Biography = actor.Biography;
                existing.Birthday = actor.Birthday;
                existing.ProfilePath = actor.ProfilePath;
            }
            await _appDbContext.SaveChangesAsync();

            return existing;
        }
        public async Task<IEnumerable<Movie>> FetchAndSaveMoviesForActorsAsync(int actorId)
        {
            var api_url = $"https://api.themoviedb.org/3/person/{actorId}/movie_credits?api_key={_apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(api_url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch actor. Status: {response.StatusCode}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TmdbMovieResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var movies = tmdbResponse.Results.Select(tmdb => tmdb.FromTmdbDtoToMovie()).ToList() ?? new List<Movie>();

            var existingIds = await _appDbContext.Movies.Select(m => m.Id).ToListAsync();

            var newMovies = movies.Where(m => !existingIds.Contains(m.Id)).ToList();

            if (newMovies.Any())
            {
                try
                {
                    await _appDbContext.AddRangeAsync(newMovies);
                    await _appDbContext.SaveChangesAsync();
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            
            var allMovies = await _appDbContext.Movies.Include(m => m.MovieGenres).ToListAsync();
            var genres = await _appDbContext.Genres.ToListAsync();

            var actor = await _appDbContext.Actors.FirstOrDefaultAsync(a => a.Id == actorId);

            if (actor == null)
            {
                Console.WriteLine($"Actor with ID {actorId} not found in database.");
            }

            foreach (var movie in movies)
            {
                var savedMovie = allMovies.FirstOrDefault(am => am.Id == movie.Id);

                if (savedMovie == null)
                    continue;

                bool alreadyLinked = await _appDbContext.ActorMovies.AnyAsync(am => am.ActorId == actor.Id && am.MovieId == savedMovie.Id);

                if (!alreadyLinked)
                {
                    var movieActor = new ActorMovies
                    {
                        MovieId = savedMovie.Id,
                        ActorId = actor.Id,
                    };

                    _appDbContext.ActorMovies.Add(movieActor);
                }

                foreach (var movieGenre in movie.MovieGenres)
                {
                    var savedGenre = genres.FirstOrDefault(g => g.Id == movieGenre.GenreId);
                    if (savedGenre == null)
                        continue;
                 
                    if (savedMovie.MovieGenres == null)
                    {
                        savedMovie.MovieGenres = new List<GenreMovies>();
                    }

                    if (!savedMovie.MovieGenres.Any(gm => gm.GenreId == savedGenre.Id))
                    {
                        var genreMovie = new GenreMovies
                        {
                            MovieId = savedMovie.Id,
                            GenreId = savedGenre.Id,
                        };

                        savedMovie.MovieGenres.Add(genreMovie);
                    }
                }
            }
            await _appDbContext.SaveChangesAsync();

            return newMovies;
        }

        public async Task<IEnumerable<Genre>> FetchAndSaveGenresAsync()
        {
            var url = $"https://api.themoviedb.org/3/genre/movie/list?api_key={_apiKey}&language=en-US";

            var response = await _httpClient.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();

            var tmdbResponse = JsonSerializer.Deserialize<TmdbGenreResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var genres = tmdbResponse?.Genres.Select(g => g.FromTMDBGenreDtoToGenre()).ToList() ?? new List<Genre>();

            var existingIds = await _appDbContext.Genres.Select(g => g.Id).ToListAsync();

            var newGenres = genres.Where(ng => !existingIds.Contains(ng.Id)).ToList();

            if(newGenres.Any())
            {
                try
                {
                    await _appDbContext.AddRangeAsync(newGenres);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            await _appDbContext.SaveChangesAsync();
            return newGenres;
        }
    }
}


