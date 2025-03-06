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
    }
}
