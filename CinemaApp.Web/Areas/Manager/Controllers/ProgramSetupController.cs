using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Cinema;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ProgramSetupController : Controller
    {
        private readonly IMovieService _movieService;

        public ProgramSetupController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index(Guid cinemaId)
        {
            var movies = await _movieService.GetMoviesForProgramAsync(cinemaId);

            if (!movies.Any())
            {
                return RedirectToAction("Index", "Movies");
            }

            ViewBag.CinemaId = cinemaId;
            return View(movies);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProgramChanges(ProgramSetupUpdateViewModel model)
        {
            if (model == null || model.Movies == null)
            {
                return RedirectToAction("Index", "ProgramSetup");
            }

            foreach (var movie in model.Movies)
            {
                if (movie.IsIncluded)
                {
                    await _movieService.AddMovieToCinemaIfNotExistsAsync(model.CinemaId, movie.MovieId);
                }
                else
                {
                    await _movieService.RemoveMovieFromCinemaIfExistsAsync(model.CinemaId, movie.MovieId);
                }
            }

            return RedirectToAction("Index", "ProgramSetup", new { cinemaId = model.CinemaId });
        }
    }
}
