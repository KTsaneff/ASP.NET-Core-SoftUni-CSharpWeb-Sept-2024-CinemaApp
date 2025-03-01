namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using CinemaApp.Services.Data.Interfaces;
    using CinemaApp.Web.ViewModels.Movie;

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MovieManagementController : Controller
    {
        private readonly IMovieService movieService;

        public MovieManagementController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public async Task<IActionResult> Manage()
        {
            var movies = await this.movieService.GetAllMoviesForAdminAsync();
            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddMovieInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await this.movieService.AddMovieAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "An error occurred while adding the movie.");
                return View(model);
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            EditMovieFormModel? movie = await this.movieService.GetMovieForEditByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMovieFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await this.movieService.EditMovieAsync(model);

            if (!success)
            {
                ModelState.AddModelError("", "An error occurred while editing the movie.");
                return View(model);
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDelete(Guid id)
        {
            bool isDeleted = await this.movieService.ToggleDeleteMovieAsync(id);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "Error deleting movie.";
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
