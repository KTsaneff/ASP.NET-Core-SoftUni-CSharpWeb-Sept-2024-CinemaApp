﻿namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.Movie;

    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {               
            return this.View(await this.movieService.GetAllMoviesAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? id)
        {
            if (!Guid.TryParse(id, out Guid movieGuid))
            {
                return this.RedirectToAction(nameof(Index));
            }

            MovieDetailsViewModel? movie = await this.movieService
                .GetMovieDetailsByIdAsync(movieGuid);

            if (movie == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(movie);
        }

        /// <summary>
        /// Loads movie details for the modal via AJAX.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailsPartial(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.NotFound();
            }

            MovieDetailsViewModel? movie = await this.movieService
                .GetMovieDetailsByIdAsync(id);

            if (movie == null)
            {
                return this.NotFound();
            }

            return PartialView("_MovieDetailsPartial", movie);
        }
    }
}
