namespace CinemaApp.Services.Data.Interfaces
{
    using CinemaApp.Web.ViewModels.Showtime;
    using Web.ViewModels.Cinema;

    public interface ICinemaService
    {
        Task<IEnumerable<CinemaIndexViewModel>> IndexGetAllOrderedByLocationAsync();

        Task AddCinemaAsync(AddCinemaFormModel model);

        Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid id);

        Task<EditCinemaFormModel?> GetCinemaForEditByIdAsync(Guid id);

        Task<bool> EditCinemaAsync(EditCinemaFormModel model);

        Task<CinemaProgramViewModel?> GetCinemaProgramByIdAsync(Guid id);

        Task<bool> ToggleDeleteCinemaAsync(Guid id);

        Task<IEnumerable<CinemaIndexViewModel>> GetAllCinemasForAdminAsync();

        Task<IEnumerable<CinemaIndexViewModel>> GetCinemasWithMoviesIncludedInProgramAsync();

        Task<IEnumerable<ShowtimeViewModel>> GetMoviesWithShowtimesAsync(Guid cinemaId);
        Task<bool> UpdateShowtimeAsync(ShowtimeViewModel model);
    }
}
