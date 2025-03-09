namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Web.ViewModels.Cinema;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;

    public class CinemaController : BaseController
    {
        private readonly ICinemaService cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var cinemas = await this.cinemaService.GetAllCinemasAsync();

            return this.View(cinemas ?? new List<UsersCinemaIndexViewModel>());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Program(Guid id)
        {
            var cinema = await this.cinemaService.GetCinemaForUserProgramById(id);
            var movies = await this.cinemaService.GetUserProgramAsync(id);

            if (cinema == null || movies == null || !movies.Any())
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            ViewData["CinemaData"] = cinema.ToString();

            return this.View(movies);
        }
    }
}
