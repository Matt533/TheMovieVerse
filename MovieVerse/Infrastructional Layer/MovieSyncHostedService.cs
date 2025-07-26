

using MovieVerse.Domain_Layer.Interfaces.IService;

namespace MovieVerse.Infrastructional_Layer
{
    public class MovieSyncHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public MovieSyncHostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var tmdbService = scope.ServiceProvider.GetRequiredService<ITMDbService>();

                try
                {
                    Console.WriteLine("Starting initial movie sync...");
                    await tmdbService.SyncMovies(5);
                    await tmdbService.SyncGenres();
                    await tmdbService.SyncActors(5);
                    Console.WriteLine("Initial movie sync completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Sync failed: {ex.Message}");
                }
            }
                
        }
    }
}
