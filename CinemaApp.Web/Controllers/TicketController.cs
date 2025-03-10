namespace CinemaApp.Web.Controllers
{
    using global::CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.Tickets;

    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly ICinemaService cinemaService;

        public TicketController(ITicketService ticketService, ICinemaService cinemaService)
        {
            this.ticketService = ticketService;
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(this.User.GetUserId());

            if (userId == Guid.Empty)
            {
                return RedirectToAction("Index", "Home");
            }

            var tickets = await this.ticketService.GetUserTicketsAsync(userId);

            return View(tickets);
        }     
    }
}
