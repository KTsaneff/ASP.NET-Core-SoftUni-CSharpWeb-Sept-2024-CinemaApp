using CinemaApp.Data.Models;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.Infrastructure.Extensions;
using CinemaApp.Web.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("Manager/api/[controller]")]
    public class TicketApiController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly ICinemaService cinemaService;

        public TicketApiController(
            ITicketService ticketService,
            ICinemaService cinemaService)
        {
            this.ticketService = ticketService;
            this.cinemaService = cinemaService;
        }

        [HttpGet("GetMoviesByCinema/{cinemaId}")]
        public async Task<IActionResult> GetMoviesByCinema(Guid cinemaId)
        {
            var cinemaProgram = await this.cinemaService.GetCinemaProgramByIdAsync(cinemaId);

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

            var result = await this.ticketService.SetAvailableTicketsAsync(model);
            if (!result)
            {
                return BadRequest("Failed to update available tickets. Please try again.");
            }

            return Ok("Ticket availability updated successfully.");
        }
    }
}
