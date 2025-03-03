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

            var model = new ProgramSetupUpdateViewModel
            {
                CinemaId = cinemaId,
                Movies = movies.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProgramChanges(ProgramSetupUpdateViewModel model)
        {
            Console.WriteLine("------ SaveProgramChanges Called ------");
            Console.WriteLine($"Cinema ID: {model.CinemaId}");

            if (model == null || model.Movies == null)
            {
                Console.WriteLine("❌ Model is null or Movies list is empty!");
                return BadRequest("Invalid data.");
            }

            foreach (var movie in model.Movies)
            {
                Console.WriteLine($"📽 Movie ID: {movie.MovieId}, IsIncluded: {movie.IsIncluded}");

                if (movie.IsIncluded)
                {
                    Console.WriteLine($"✅ Adding movie {movie.MovieId} to Cinema {model.CinemaId}");
                    await _movieService.AddMovieToCinemaIfNotExistsAsync(model.CinemaId, movie.MovieId);
                }
                else
                {
                    Console.WriteLine($"🗑 Removing movie {movie.MovieId} from Cinema {model.CinemaId}");
                    await _movieService.RemoveMovieFromCinemaIfExistsAsync(model.CinemaId, movie.MovieId);
                }
            }

            return RedirectToAction("Index", new { cinemaId = model.CinemaId });
        }

    }
}
