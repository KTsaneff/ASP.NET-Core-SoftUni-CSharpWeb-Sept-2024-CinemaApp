using System.ComponentModel.DataAnnotations;

using CinemaApp.Data.Models;
using CinemaApp.Services.Mapping;

using AutoMapper;

namespace CinemaApp.Web.ViewModels.Tickets
{
    public class BuyTicketViewModel : IMapTo<Ticket>
    {
        [Required]
        public Guid CinemaId { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select at least one ticket.")]
        public int NumberOfTickets { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be a positive amount.")]
        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BuyTicketViewModel, Ticket>();
        }
    }
}
