using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly ICinemaService _cinemaService;
        private readonly IManagerService _managerService;

        public TicketController(
            ITicketService ticketService,
            ICinemaService cinemaService,
            IManagerService managerService)
        {
            _ticketService = ticketService;
            _cinemaService = cinemaService;
            _managerService = managerService;
        }

        public async Task<IActionResult> Index()
        {
            var cinemas = await _cinemaService.IndexGetAllOrderedByLocationAsync();
            return View(cinemas.ToList());
        }

        [HttpGet("GetMoviesByCinema/{cinemaId}")]
        public async Task<IActionResult> GetMoviesByCinema(Guid cinemaId)
        {
            var cinemaProgram = await _cinemaService.GetCinemaProgramByIdAsync(cinemaId);
            if (cinemaProgram == null)
            {
                return NotFound("Cinema not found.");
            }

            return Ok(cinemaProgram.Movies);
        }

        [HttpPost("UpdateAvailableTickets")]
        public async Task<IActionResult> UpdateAvailableTickets([FromBody] SetAvailableTicketsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ticketService.SetAvailableTicketsAsync(model);
            if (!result)
            {
                return BadRequest("Failed to update available tickets. Please try again.");
            }

            return Ok(new { success = true, message = "Ticket availability updated successfully." });
        }
    }
}
