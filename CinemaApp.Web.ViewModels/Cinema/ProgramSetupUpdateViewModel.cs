using CinemaApp.Web.ViewModels.Movie;

namespace CinemaApp.Web.ViewModels.Cinema
{
    public class ProgramSetupUpdateViewModel
    {
        public Guid CinemaId { get; set; }

        public IEnumerable<ProgramSetupViewModel> Movies { get; set; } = null!;
    }
}
