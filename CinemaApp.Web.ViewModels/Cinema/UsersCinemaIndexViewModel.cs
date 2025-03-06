namespace CinemaApp.Web.ViewModels.Cinema
{
    public class UsersCinemaIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public bool HasMovies { get; set; } = false;
    }
}
