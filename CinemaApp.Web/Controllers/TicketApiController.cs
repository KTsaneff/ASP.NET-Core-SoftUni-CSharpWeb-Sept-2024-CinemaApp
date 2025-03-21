﻿using CinemaApp.Data.Models;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.Infrastructure.Extensions;
using CinemaApp.Web.ViewModels.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketApiController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly ICinemaService cinemaService;
        private readonly IManagerService managerService;

        public TicketApiController(
            ITicketService ticketService,
            ICinemaService cinemaService,
            IManagerService managerService)
        {
            this.ticketService = ticketService;
            this.cinemaService = cinemaService;
            this.managerService = managerService;
        }
        
        [HttpPost("BuyTicket")]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = this.User.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            if (await IsUserManagerAsync())
            {
                return Forbid("Managers are not allowed to buy tickets.");
            }

            var viewModel = new BuyTicketViewModel
            {
                CinemaId = request.CinemaId,
                MovieId = request.MovieId,
                NumberOfTickets = request.Quantity
            };

            var result = await this.ticketService.BuyTicketAsync(viewModel, Guid.Parse(userId));
            if (!result)
            {
                return BadRequest("Failed to purchase tickets. Not enough tickets available.");
            }

            return Ok("Ticket(s) purchased successfully.");
        }

            private async Task<bool> IsUserManagerAsync()
        {
            var userId = this.User.GetUserId();
            return !string.IsNullOrWhiteSpace(userId) && await this.managerService.IsUserManagerAsync(userId);
        }
    }
}
