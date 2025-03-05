namespace CinemaApp.Web.ViewModels.Cinema
{
    public class UsersCinemaIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int MovieCount { get; set; }

        public bool HasMovies => this.MovieCount > 0;

        public string ImagePath => $"/images/{this.ImageUrl}";
    }
}
