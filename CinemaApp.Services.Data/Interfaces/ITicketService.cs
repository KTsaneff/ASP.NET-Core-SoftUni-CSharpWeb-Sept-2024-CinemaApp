using CinemaApp.Web.ViewModels.Tickets;

namespace CinemaApp.Services.Data.Interfaces
{
    public interface ITicketService
    {
        Task<bool> BuyTicketAsync(BuyTicketViewModel model);
        Task<IEnumerable<UserTicketViewModel>> GetUserTicketsAsync(Guid userId);
        Task<bool> SetAvailableTicketsAsync(Guid cinemaId, Guid movieId, int availableTickets);

        Task<bool> DecreaseAvailableTicketsAsync(Guid cinemaId, Guid movieId, int numberOfTickets);
    }
}
