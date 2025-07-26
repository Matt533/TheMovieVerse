using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Data_Layer
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ActorsOverview> ActorsOverview { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovies> ActorMovies { get; set; }
        public DbSet<UserMovieFavorite> MoviesFavorite { get; set; }
        public DbSet<UserMovieWatch> MoviesWatch { get; set; }
        public DbSet<GenreMovies> GenreMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1a111111-1111-1111-1111-111111111111",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2b222222-2222-2222-2222-222222222222",
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<ActorMovies>()
                .HasKey(am => new { am.ActorId, am.MovieId });

            modelBuilder.Entity<ActorMovies>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.ActorsList)
                .HasForeignKey(am => am.MovieId);

            modelBuilder.Entity<ActorMovies>()
            .HasOne(am => am.Actor)
            .WithMany(a => a.PlayedInMovies)
            .HasForeignKey(am => am.ActorId);

            modelBuilder.Entity<GenreMovies>()
                .HasKey(gm => new { gm.MovieId, gm.GenreId });

            modelBuilder.Entity<GenreMovies>()
                .HasOne(gm => gm.Genre)
                .WithMany(g => g.GenreMovies)
                .HasForeignKey(gm => gm.GenreId);

            modelBuilder.Entity<GenreMovies>()
                .HasOne(gm => gm.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(gm => gm.MovieId);

            modelBuilder.Entity<UserMovieWatch>()
                .HasKey(umw => new { umw.MovieId, umw.UserId });

            modelBuilder.Entity<UserMovieWatch>()
                .HasOne(mwu => mwu.User)
                .WithMany(mwu => mwu.WatchedMovies)
                .HasForeignKey(mwu => mwu.UserId);

            modelBuilder.Entity<UserMovieWatch>()
                .HasOne(mwu => mwu.Movie)
                .WithMany(mwu => mwu.WatchedByUsers)
                .HasForeignKey(mwu => mwu.MovieId);


            modelBuilder.Entity<UserMovieFavorite>()
                .HasKey(umf => new { umf.MovieId, umf.UserId });

            modelBuilder.Entity<UserMovieFavorite>()
                .HasOne(mfu => mfu.User)
                .WithMany(mfu => mfu.FavoriteMovies)
                .HasForeignKey(mfu => mfu.UserId);

            modelBuilder.Entity<UserMovieFavorite>()
                .HasOne(mfu => mfu.Movie)
                .WithMany(mfu => mfu.FavoritedByUsers)
                .HasForeignKey(mfu => mfu.MovieId);
        }
    }
}
