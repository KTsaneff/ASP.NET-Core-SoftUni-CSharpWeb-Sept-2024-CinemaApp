namespace CinemaApp.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Interfaces;
    using Interfaces;
    using Mapping;
    using Web.ViewModels.Cinema;
    using Web.ViewModels.Movie;

    public class CinemaService : BaseService, ICinemaService
    {
        private readonly IRepository<Cinema, Guid> cinemaRepository;

        public CinemaService(IRepository<Cinema, Guid> cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> IndexGetAllOrderedByLocationAsync()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this.cinemaRepository
                .GetAllAttached()
                .OrderBy(c => c.Location)
                .To<CinemaIndexViewModel>()
                .ToArrayAsync();

            return cinemas;
        }

        public async Task AddCinemaAsync(AddCinemaFormModel model)
        {
            Cinema cinema = new Cinema();
            AutoMapperConfig.MapperInstance.Map(model, cinema);

            await this.cinemaRepository.AddAsync(cinema);
        }

        public async Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid id)
        {
            Cinema? cinema = await this.cinemaRepository
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
            return await this.cinemaRepository
                .GetAllAttached()
                .Where(c => c.Id == id)
                .To<EditCinemaFormModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditCinemaAsync(EditCinemaFormModel model)
        {
            Cinema? cinemaEntity = await this.cinemaRepository.GetByIdAsync(Guid.Parse(model.Id));

            if (cinemaEntity == null)
            {
                return false;
            }

            AutoMapperConfig.MapperInstance.Map(model, cinemaEntity);

            return await this.cinemaRepository.UpdateAsync(cinemaEntity);
        }

        public async Task<CinemaProgramViewModel?> GetCinemaProgramByIdAsync(Guid id)
        {
            Cinema? cinema = await this.cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);

            CinemaProgramViewModel? viewModel = null;
            if (cinema != null)
            {
                viewModel = new CinemaProgramViewModel()
                {
                    Id = cinema.Id.ToString(),
                    Name = cinema.Name,
                    Location = cinema.Location,
                    Movies = cinema.CinemaMovies
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
                        })
                        .ToList()
                };
            }

            return viewModel;
        }

        public async Task<bool> ToggleDeleteCinemaAsync(Guid id)
        {
            Cinema? cinema = await this.cinemaRepository.GetByIdAsync(id);

            if (cinema == null)
            {
                return false;
            }

            cinema.IsDeleted = !cinema.IsDeleted;
            await this.cinemaRepository.UpdateAsync(cinema);

            return true;
        }

        public async Task<IEnumerable<CinemaIndexViewModel>> GetAllCinemasForAdminAsync()
        {
            return await this.cinemaRepository
                .GetAllAttached()
                .OrderBy(c => c.Location)
                .To<CinemaIndexViewModel>()
                .ToArrayAsync();
        }
    }
}
