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

        public Guid UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You can buy between 1 and 10 tickets.")]
        public int NumberOfTickets { get; set; }

        [Required]
        [Range(0.01, int.MaxValue, ErrorMessage = "The price must be a positive amount (up to 1000).")]
        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BuyTicketViewModel, Ticket>()
                         .ForMember(t => t.UserId, opt => opt.Ignore());
        }
    }
}
