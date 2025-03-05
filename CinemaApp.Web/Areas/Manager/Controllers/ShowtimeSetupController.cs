using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ShowtimeSetupController : Controller
    {
        private readonly ICinemaService _cinemaService;

        public ShowtimeSetupController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaService.GetCinemasWithMoviesIncludedInProgramAsync();

            return View(cinemas);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesWithShowtimes(Guid cinemaId)
        {
            var movies = await _cinemaService.GetMoviesWithShowtimesAsync(cinemaId);
            return Ok(movies);
        }
    }
}
