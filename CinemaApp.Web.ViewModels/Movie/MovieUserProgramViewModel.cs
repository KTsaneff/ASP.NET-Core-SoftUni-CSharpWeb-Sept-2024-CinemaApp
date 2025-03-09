namespace CinemaApp.Web.ViewModels.Movie
{
    public class MovieUserProgramViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Director { get; set; } = null!;

        public  string? ImageUrl { get; set; }
    }
}
