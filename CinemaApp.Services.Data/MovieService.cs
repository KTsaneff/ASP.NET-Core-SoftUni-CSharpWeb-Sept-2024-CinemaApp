namespace CinemaApp.Services.Data
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Interfaces;
    using Interfaces;
    using Mapping;
    using Web.ViewModels.Cinema;
    using Web.ViewModels.Movie;

    using static Common.EntityValidationConstants.Movie;

    public class MovieService : BaseService, IMovieService
    {
        private readonly IRepository<Movie, Guid> movieRepository;
        private readonly IRepository<Cinema, Guid> cinemaRepository;
        private readonly IRepository<CinemaMovie, object> cinemaMovieRepository;

        public MovieService(IRepository<Movie, Guid> movieRepository,
            IRepository<Cinema, Guid> cinemaRepository,
            IRepository<CinemaMovie, object> cinemaMovieRepository)
        {
            this.movieRepository = movieRepository;
            this.cinemaRepository = cinemaRepository;
            this.cinemaMovieRepository = cinemaMovieRepository;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            return await movieRepository
                .GetAllAttached()
                .To<AllMoviesIndexViewModel>()
                .ToArrayAsync();
        }

        public async Task<bool> AddMovieAsync(AddMovieInputModel inputModel)
        {
            bool isReleaseDateValid = DateTime
                .TryParseExact(inputModel.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime releaseDate);
            if (!isReleaseDateValid)
            {
                return false;
            }

            Movie movie = new Movie();
            AutoMapperConfig.MapperInstance.Map(inputModel, movie);
            movie.ReleaseDate = releaseDate;

            await this.movieRepository.AddAsync(movie);

            return true;
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id)
        {
            Movie? movie = await this.movieRepository
                .GetByIdAsync(id);
            MovieDetailsViewModel? viewModel = new MovieDetailsViewModel();
            if (movie != null)
            {
                AutoMapperConfig.MapperInstance.Map(movie, viewModel);
            }

            return viewModel;
        }

        public async Task<AddMovieToCinemaInputModel?> GetAddMovieToCinemaInputModelByIdAsync(Guid id)
        {
            Movie? movie = await this.movieRepository
                .GetByIdAsync(id);
            AddMovieToCinemaInputModel? viewModel = null;
            if (movie != null)
            {
                viewModel = new AddMovieToCinemaInputModel()
                {
                    Id = id.ToString(),
                    MovieTitle = movie.Title,
                    Cinemas = await this.cinemaRepository
                        .GetAllAttached()
                        .Include(c => c.CinemaMovies)
                        .ThenInclude(cm => cm.Movie)
                        .Select(c => new CinemaCheckBoxItemInputModel()
                        {
                            Id = c.Id.ToString(),
                            Name = c.Name,
                            Location = c.Location,
                            IsSelected = c.CinemaMovies
                                .Any(cm => cm.Movie.Id == id &&
                                           cm.IsDeleted == false)
                        })
                        .ToArrayAsync()
                };
            }

            return viewModel;
        }

        public async Task<IEnumerable<MovieIndexViewModel>> GetAllMoviesForAdminAsync()
        {
            return await this.movieRepository
                .GetAllAttached()
                .To<MovieIndexViewModel>()
                .ToArrayAsync();
        }

        public async Task<EditMovieFormModel?> GetMovieForEditByIdAsync(Guid id)
        {
            return await this.movieRepository
                .GetAllAttached()
                .Where(m => m.Id == id)
                .To<EditMovieFormModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditMovieAsync(EditMovieFormModel model)
        {
            Movie? movie = await this.movieRepository
                .GetByIdAsync(model.Id);

            if (movie == null)
            {
                return false;
            }

            AutoMapperConfig.MapperInstance.Map(model, movie);
            await this.movieRepository.UpdateAsync(movie);

            return true;
        }

        public async Task<bool> ToggleDeleteMovieAsync(Guid id)
        {
            Movie? movie = await this.movieRepository
                .GetByIdAsync(id);
            if (movie == null)
            {
                return false;
            }
            movie.IsDeleted = !movie.IsDeleted;
            await this.movieRepository.UpdateAsync(movie);
            return true;
        }
        public async Task<IEnumerable<ProgramSetupViewModel>> GetMoviesForProgramAsync(Guid cinemaId)
        {
            var moviesWithStatus = await this.movieRepository
                .GetAllAttached()
                .Where(m => !m.IsDeleted)
                .Select(m => new ProgramSetupViewModel
                {
                    MovieId = m.Id,
                    Title = m.Title,
                    Duration = m.Duration,
                    PosterUrl = m.ImageUrl,
                    IsIncluded = this.cinemaMovieRepository
                        .GetAllAttached()
                        .Any(cm => cm.CinemaId == cinemaId && cm.MovieId == m.Id && !cm.IsDeleted)
                })
                .OrderByDescending(m => m.IsIncluded)
                .ThenBy(m => m.Title)
                .ToListAsync();

            return moviesWithStatus;
        }

        public async Task<bool> AddMovieToCinemaIfNotExistsAsync(Guid cinemaId, Guid movieId)
        {
            var cinemaMovie = await this.cinemaMovieRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(cm => cm.CinemaId == cinemaId && cm.MovieId == movieId);

            if (cinemaMovie == null)
            {
                cinemaMovie = new CinemaMovie()
                {
                    CinemaId = cinemaId,
                    MovieId = movieId,
                    AvailableTickets = 0,
                    IsDeleted = false
                };

                await this.cinemaMovieRepository.AddAsync(cinemaMovie);
            }
            else
            {
                cinemaMovie.IsDeleted = false;
                await this.cinemaMovieRepository.UpdateAsync(cinemaMovie);
            }
            return true;
        }

        public async Task<bool> RemoveMovieFromCinemaIfExistsAsync(Guid cinemaId, Guid movieId)
        {
            var cinemaMovie = await this.cinemaMovieRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(cm => cm.CinemaId == cinemaId && cm.MovieId == movieId);

            if (cinemaMovie != null && !cinemaMovie.IsDeleted)
            {
                cinemaMovie.IsDeleted = true;
                await this.cinemaMovieRepository.UpdateAsync(cinemaMovie);
            }

            return true;
        }
    }
}
