﻿namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.Tickets;

    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly ICinemaService cinemaService;

        public TicketController(ITicketService ticketService, ICinemaService cinemaService, IManagerService managerService)
            : base(managerService)
        {
            this.ticketService = ticketService;
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyTicket(Guid cinemaId, Guid movieId)
        {
            var userId = this.User.GetUserId();

            bool isManager = await this.IsUserManagerAsync();

            if (string.IsNullOrWhiteSpace(userId) || isManager)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new BuyTicketViewModel
            {
                CinemaId = cinemaId,
                MovieId = movieId,
                UserId = Guid.Parse(userId)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyTicket(BuyTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await this.ticketService.BuyTicketAsync(model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unable to purchase ticket. Please try again.");
                return View(model);
            }

            return RedirectToAction(nameof(MyTickets));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyTickets()
        {
            Guid userId = Guid.Parse(this.User.GetUserId());

            if(userId == Guid.Empty)
            {
                return RedirectToAction("Index", "Home");
            }

            var tickets = await this.ticketService.GetUserTicketsAsync(userId);

            return View(tickets);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction("Index", "Home");
            }

            var cinemas = await this.cinemaService.IndexGetAllOrderedByLocationAsync();
            return View(cinemas);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SetAvailableTickets(Guid cinemaId, Guid movieId)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new SetAvailableTicketsViewModel
            {
                CinemaId = cinemaId,
                MovieId = movieId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetAvailableTickets(SetAvailableTicketsViewModel model)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await this.ticketService.SetAvailableTicketsAsync(model.CinemaId, model.MovieId, model.AvailableTickets);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unable to set available tickets. Please try again.");
                return View(model);
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
