namespace CinemaApp.Web.ViewModels.Cinema
{
    public class UserProgramCinemaViewModel
    {
        public Guid Id { get; set; }

        public string CinemaName { get; set; } = null!;

        public string CinemaLocation { get; set; } = null!;

        public override string ToString()
        {
            return $"{this.CinemaName} - {this.CinemaLocation}";
        }
    }
}
