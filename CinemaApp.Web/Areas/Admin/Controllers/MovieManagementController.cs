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

            await this.movieService.AddMovieAsync(model);
            return RedirectToAction(nameof(Manage));
        }

        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var movie = await this.movieService.GetMovieByIdAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movie);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(EditMovieInputModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    bool isUpdated = await this.movieService.UpdateMovieAsync(model);
        //    if (!isUpdated)
        //    {
        //        ModelState.AddModelError("", "Unexpected error occurred while updating the movie.");
        //        return View(model);
        //    }

        //    return RedirectToAction(nameof(Manage));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            //bool isDeleted = await this.movieService.DeleteMovieAsync(id);
            //if (!isDeleted)
            //{
            //    TempData["ErrorMessage"] = "Error deleting movie.";
            //}

            return RedirectToAction(nameof(Manage));
        }
    }
}
