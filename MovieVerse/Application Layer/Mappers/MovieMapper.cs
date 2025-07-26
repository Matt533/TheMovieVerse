using MovieVerse.Application_Layer.DTOs;
using MovieVerse.Application_Layer.DTOs.ActorDto;
using MovieVerse.Application_Layer.DTOs.GenreDto;
using MovieVerse.Application_Layer.DTOs.UserDtos;
using MovieVerse.Domain_Layer.DTOs;
using MovieVerse.Domain_Layer.Enumeration;
using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Infrastructional_Layer.Mappers
{
    public static class MovieMapper
    {
        public static Movie FromMovieDtoToModel(this CreateMovieDto createMovieDto)
        {

            return new Movie
            { 
                Language = createMovieDto.Language,
                Title = createMovieDto.Title,
                Description = createMovieDto.Description,
                Rating = createMovieDto.Rating,
                PosterUrl = createMovieDto.PosterUrl,
                ReleaseDate = createMovieDto.ReleaseDate,
            };
        }

        public static MovieDto FromMovieToMovieDTO(this Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Language = movie.Language,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate,

                Genres = movie.MovieGenres != null
                ? movie.MovieGenres.Where(mg => mg.Genre.Name != null)
                .Select(mg => mg.Genre.Name)
                .ToList() : new List<string>()
            }; 
        }
        public static MovieDto FromMovieToMovieDTOForGenres(this Movie movie, string selectedGenre)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Language = movie.Language,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate,

                Genres = movie.MovieGenres
                .Where(mg => mg.Genre != null && mg.Genre.Name.Equals(selectedGenre))
                .Select(mg => mg.Genre.Name)
                .ToList()
            };
        }

        public static string FromEnumToString(this MovieCategory movieCategory)
        {
            switch(movieCategory)
            {
                case MovieCategory.NowPlaying:
                    return "now_playing";
                case MovieCategory.Popular:
                    return "popular";
                case MovieCategory.TopRated:
                    return "top_rated";
                case MovieCategory.Upcoming:
                    return "upcoming";
                default:
                    throw new ArgumentException("Invalid category!");
            }
        }

        public static Movie FromTmdbDtoToMovie(this TMDBMovieDto tMDBMovieDto)
        {
            return new Movie
            {
                Id = tMDBMovieDto.TmdbId,
                Language = tMDBMovieDto.Language,
                Title = tMDBMovieDto.Title,
                Description = tMDBMovieDto.Description,
                Rating = tMDBMovieDto.Rating,
                PosterUrl = $"https://image.tmdb.org/t/p/w500{tMDBMovieDto.PosterUrl}",
                ReleaseDate = tMDBMovieDto.ReleaseDate,
                MovieGenres = tMDBMovieDto.GenreIds.Select(id => new GenreMovies
                {
                    GenreId = id,
                    MovieId = tMDBMovieDto.TmdbId
                }).ToList()
            };
        }

        public static ActorsOverview FromActorOverviewDtoToActorOverview(this ActorOverviewDto actorOverviewDto)
        {
            return new ActorsOverview
            {
                Id = actorOverviewDto.Id,
                Name = actorOverviewDto.Name,
                Avatar = $"https://image.tmdb.org/t/p/w500{actorOverviewDto.Avatar}"
            };
        }

        public static ActorDto FromActorToActorDto(this Actor actor)
        {
            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                ProfilePath = actor.ProfilePath,
                Biography = actor.Biography,
                Birthday = actor.Birthday,

                Movies = actor.PlayedInMovies
                .Where(pim => pim.Movie != null)
                .Select(pim => pim.Movie.FromMovieToMovieDTO())
                .ToList()
            };
        }

        public static Actor FromActorDtoToModel(this CreateActorDto actorDto)
        {
            return new Actor
            {
                Name = actorDto.Name,
                Biography = actorDto.Biography,
                Birthday = actorDto.Birthday,
                ProfilePath = actorDto.ProfilePath,
            };
        }

        public static ActorOverviewDto FromActorOverviewToActorOverviewDto(this ActorsOverview actorsOverview)
        {
            return new ActorOverviewDto
            {
                Id = actorsOverview.Id,
                Name = actorsOverview.Name,
                Avatar = actorsOverview.Avatar
            };
        }

        public static Genre FromTMDBGenreDtoToGenre(this TMDBGenreDto tmdbGenreDto)
        {
            return new Genre
            {
                Id = tmdbGenreDto.TmdbId,
                Name = tmdbGenreDto.Name,
            };
        }
        public static GenreDto FromGenreToDto(this Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public static UserDto FromUserToDto(this User user)
        {
            return new UserDto
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
