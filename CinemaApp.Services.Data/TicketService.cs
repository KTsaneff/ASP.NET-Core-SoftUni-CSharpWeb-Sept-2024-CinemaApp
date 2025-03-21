﻿using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Services.Mapping;
using CinemaApp.Web.ViewModels.Tickets;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket, Guid> ticketRepository;
        private readonly IRepository<Cinema, Guid> cinemaRepository;
        private readonly IRepository<Movie, Guid> movieRepository;
        private readonly IRepository<CinemaMovie, object> cinemaMovieRepository;

        public TicketService(
            IRepository<Ticket, Guid> ticketRepository,
            IRepository<Cinema, Guid> cinemaRepository,
            IRepository<Movie, Guid> movieRepository,
            IRepository<CinemaMovie, object> cinemaMovieRepository)
        {
            this.ticketRepository = ticketRepository;
            this.cinemaRepository = cinemaRepository;
            this.movieRepository = movieRepository;
            this.cinemaMovieRepository = cinemaMovieRepository;
        }

        public async Task<bool> BuyTicketAsync(BuyTicketViewModel model, Guid userId)
        {
            var cinemaMovie = await this.cinemaMovieRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(cm => cm.MovieId == model.MovieId && cm.CinemaId == model.CinemaId);

            if (cinemaMovie == null || cinemaMovie.AvailableTickets < model.NumberOfTickets)
            {
                return false;
            }

            for (int i = 0; i < model.NumberOfTickets; i++)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    CinemaId = model.CinemaId,
                    MovieId = model.MovieId,
                    UserId = userId
                };

                await this.ticketRepository.AddAsync(ticket);
            }

            cinemaMovie.AvailableTickets -= model.NumberOfTickets;

            await this.cinemaMovieRepository.SaveChangesAsync();
            await this.ticketRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserTicketViewModel>> GetUserTicketsAsync(Guid userId)
        {
            var tickets = await this.ticketRepository
                .GetAllAttached()
                .Where(t => t.UserId == userId)
                .Include(t => t.Movie)
                .Include(t => t.Cinema)
                .ToListAsync();

            var groupedTickets = tickets
                .Where(t => t.Movie != null && t.Cinema != null)
                .GroupBy(t => new { Title = t.Movie.Title, CinemaName = t.Cinema.Name })
                .Select(g => new UserTicketViewModel
                {
                    MovieTitle = g.Key.Title,
                    CinemaName = g.Key.CinemaName,
                    ImageUrl = g.First().Movie.ImageUrl,
                    TicketCount = g.Count(),
                    Price = g.Sum(t => t.Price)
                })
                .ToList();

            return groupedTickets;
        }




        public async Task<bool> SetAvailableTicketsAsync(SetAvailableTicketsViewModel model)
        {
            var cinemaMovie = await this.cinemaMovieRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(cm => cm.CinemaId == model.CinemaId && cm.MovieId == model.MovieId);

            if (cinemaMovie == null)
            {
                return false;
            }

            cinemaMovie.AvailableTickets = model.AvailableTickets;
            await this.cinemaMovieRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DecreaseAvailableTicketsAsync(Guid cinemaId, Guid movieId, int numberOfTickets)
        {
            var cinemaMovie = await this.cinemaMovieRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(cm => cm.CinemaId == cinemaId && cm.MovieId == movieId);

            if (cinemaMovie == null || cinemaMovie.AvailableTickets < numberOfTickets)
            {
                return false;
            }

            cinemaMovie.AvailableTickets -= numberOfTickets;
            await this.cinemaMovieRepository.SaveChangesAsync();

            return true;
        }

    }
}
