namespace CinemaApp.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Interfaces;
    using Interfaces;
    using Mapping;
    using Web.ViewModels.Cinema;
    using Web.ViewModels.Movie;
    using CinemaApp.Web.ViewModels.Showtime;

    public class CinemaService : BaseService, ICinemaService
    {
        private readonly IRepository<Cinema, Guid> _cinemaRepository;
        private readonly IRepository<Movie, Guid> _movieRepository;

        public CinemaService(
            IRepository<Cinema, Guid> cinemaRepository, 
            IRepository<Movie, Guid> movieRepository)
        {
            this._cinemaRepository = cinemaRepository;
            this._movieRepository = movieRepository;
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> IndexGetAllOrderedByLocationAsync()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this._cinemaRepository
                .GetAllAttached()
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Location)
                .ThenBy(c => c.Name)
                .Select(c => new CinemaIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    HasMovies = c.CinemaMovies.Any(cm => !cm.IsDeleted)
                })
                .ToArrayAsync();

            return cinemas;
        }

        public async Task<IEnumerable<UsersCinemaIndexViewModel>> GetAllCinemasAsync()
        {
            return await this._cinemaRepository
                .GetAllAttached()
                .Where(c => !c.IsDeleted && c.CinemaMovies.Any(cm => !cm.Movie.IsDeleted))
                .OrderBy(c => c.Location)
                .ThenBy(c => c.Name)
                .Select(c => new UsersCinemaIndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location,
                    HasMovies = c.CinemaMovies.Any(cm => !cm.Movie.IsDeleted)
                })
                .ToArrayAsync();
        }


        public async Task AddCinemaAsync(AddCinemaFormModel model)
        {
            Cinema cinema = new Cinema();
            AutoMapperConfig.MapperInstance.Map(model, cinema);

            await this._cinemaRepository.AddAsync(cinema);
        }

        public async Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid id)
        {
            Cinema? cinema = await this._cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            CinemaDetailsViewModel? viewModel = null;
            if (cinema != null)
            {
                viewModel = new CinemaDetailsViewModel()
                {
                    Name = cinema.Name,
                    Location = cinema.Location,
                    Movies = cinema.CinemaMovies
                        .Where(cm => cm.IsDeleted == false)
                        .Select(cm => new CinemaMovieViewModel()
                        {
                            Title = cm.Movie.Title,
                            Duration = cm.Movie.Duration,
                        })
                        .ToArray()
                };
            }

            return viewModel;
        }

        public async Task<EditCinemaFormModel?> GetCinemaForEditByIdAsync(Guid id)
        {
            return await this._cinemaRepository
                .GetAllAttached()
                .Where(c => c.Id == id)
                .To<EditCinemaFormModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditCinemaAsync(EditCinemaFormModel model)
        {
            Cinema? cinemaEntity = await this._cinemaRepository.GetByIdAsync(Guid.Parse(model.Id));

            if (cinemaEntity == null)
            {
                return false;
            }

            AutoMapperConfig.MapperInstance.Map(model, cinemaEntity);

            return await this._cinemaRepository.UpdateAsync(cinemaEntity);
        }

        public async Task<CinemaProgramViewModel?> GetCinemaProgramByIdAsync(Guid id)
        {
            var cinema = await this._cinemaRepository
                .GetAllAttached()
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Location,
                    Movies = c.CinemaMovies
                        .Where(cm => !cm.IsDeleted)
                        .Select(cm => new MovieInCinemaViewModel
                        {
                            Id = cm.Movie.Id.ToString(),
                            CinemaId = cm.CinemaId.ToString(),
                            Title = cm.Movie.Title,
                            Genre = cm.Movie.Genre,
                            Duration = $"{cm.Movie.Duration} min",
                            Description = cm.Movie.Description,
                            AvailableTickets = cm.AvailableTickets
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cinema == null)
            {
                return null;
            }

            return new CinemaProgramViewModel
            {
                Id = cinema.Id.ToString(),
                Name = cinema.Name,
                Location = cinema.Location,
                Movies = cinema.Movies
            };
        }


        public async Task<bool> ToggleDeleteCinemaAsync(Guid id)
        {
            Cinema? cinema = await this._cinemaRepository.GetByIdAsync(id);

            if (cinema == null)
            {
                return false;
            }

            cinema.IsDeleted = !cinema.IsDeleted;
            await this._cinemaRepository.UpdateAsync(cinema);

            return true;
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> GetAllCinemasForAdminAsync()
        {
            return await this._cinemaRepository
                .GetAllAttached()
                .OrderBy(c => c.Location)
                .To<CinemaIndexViewModel>()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> GetCinemasWithMoviesIncludedInProgramAsync()
        {
            IEnumerable<CinemaIndexViewModel> cinemasWithMovies = await this._cinemaRepository
                .GetAllAttached()
                .Where(c => !c.IsDeleted && c.CinemaMovies.Any(cm => !cm.IsDeleted))
                .OrderBy(c => c.Location)
                .ThenBy(c => c.Name)
                .Select(c => new CinemaIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    HasMovies = c.CinemaMovies.Any(cm => !cm.IsDeleted)
                })
                .ToListAsync();

            return cinemasWithMovies;
        }

        /// <summary>
        /// Retrieves all movies in a cinema along with their showtimes.
        /// </summary>
        public async Task<IEnumerable<ShowtimeViewModel>> GetMoviesWithShowtimesAsync(Guid cinemaId)
        {
            var cinema = await this._cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == cinemaId && !c.IsDeleted);

            if (cinema == null)
            {
                return Enumerable.Empty<ShowtimeViewModel>();
            }

            return cinema.CinemaMovies
                .Where(cm => !cm.IsDeleted)
                .Select(cm => new ShowtimeViewModel
                {
                    CinemaId = cm.CinemaId,
                    MovieId = cm.MovieId,
                    Title = cm.Movie.Title,
                    Showtimes = !string.IsNullOrEmpty(cm.Showtimes) && cm.Showtimes.Length == 5
                                ? cm.Showtimes.Select(c => c - '0').ToList()
                                : new List<int> { 0, 0, 0, 0, 0 }

                }).ToList();
        }


        /// <summary>
        /// Updates the showtimes for a specific movie in a cinema.
        /// </summary>
        public async Task<bool> UpdateShowtimeAsync(ShowtimeViewModel model)
        {
            var cinema = await this._cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .FirstOrDefaultAsync(c => c.Id == model.CinemaId && !c.IsDeleted);

            if (cinema == null) return false;

            var cinemaMovie = cinema.CinemaMovies.FirstOrDefault(cm => cm.MovieId == model.MovieId);
            if (cinemaMovie == null) return false;

            cinemaMovie.Showtimes = string.Join("", model.Showtimes);

            return await this._cinemaRepository.UpdateAsync(cinema);
        }

    }
}
