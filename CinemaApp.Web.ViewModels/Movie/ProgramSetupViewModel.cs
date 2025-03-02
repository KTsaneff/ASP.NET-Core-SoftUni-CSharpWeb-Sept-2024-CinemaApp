namespace CinemaApp.Web.ViewModels.Movie
{
    public class ProgramSetupViewModel
    {
        public Guid MovieId { get; set; }
        public string Title { get; set; } = null!;
        public int Duration { get; set; }
        public string? PosterUrl { get; set; } = null!;
        public bool IsIncluded { get; set; } = false;
    }
}
