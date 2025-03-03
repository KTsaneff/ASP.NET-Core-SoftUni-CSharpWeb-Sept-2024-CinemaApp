using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Cinema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketService ticketService;
        private readonly ICinemaService cinemaService;
        private readonly IMovieService movieService;

        public TicketController(
            ITicketService ticketService,
            ICinemaService cinemaService,
            IMovieService movieService)
        {
            this.ticketService = ticketService;
            this.cinemaService = cinemaService;
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await this.cinemaService
                .IndexGetAllOrderedByLocationAsync();

            return View(cinemas);
        }
    }
}
