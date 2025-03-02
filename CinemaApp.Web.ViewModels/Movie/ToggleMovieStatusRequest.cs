namespace CinemaApp.Web.ViewModels.Movie
{
    public class ToggleMovieStatusRequest
    {
        public Guid MovieId { get; set; }
        public Guid CinemaId { get; set; }
        public bool IsIncluded { get; set; }
    }
}
