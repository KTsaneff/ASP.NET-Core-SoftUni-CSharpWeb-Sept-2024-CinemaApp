namespace CinemaApp.Services.Data.Interfaces
{
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();

        Task<bool> AddMovieAsync(AddMovieInputModel inputModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<AddMovieToCinemaInputModel?> GetAddMovieToCinemaInputModelByIdAsync(Guid id);

        Task<IEnumerable<MovieIndexViewModel>> GetAllMoviesForAdminAsync();

        Task<EditMovieFormModel?> GetMovieForEditByIdAsync(Guid id);

        Task<bool> EditMovieAsync(EditMovieFormModel model);

        Task<bool> ToggleDeleteMovieAsync(Guid id);

        Task<IEnumerable<ProgramSetupViewModel>> GetMoviesForProgramAsync(Guid cinemaId);

        Task<bool> AddMovieToCinemaIfNotExistsAsync(Guid cinemaId, Guid movieId);

        Task<bool> RemoveMovieFromCinemaIfExistsAsync(Guid cinemaId, Guid movieId);
    }
}
