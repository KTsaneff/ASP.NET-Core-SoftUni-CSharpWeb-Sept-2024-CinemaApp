namespace CinemaApp.Web.Controllers
{

    namespace CinemaApp.Web.Controllers
    {
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Identity;
        using Microsoft.AspNetCore.Mvc;

        using Data.Models;
        using Services.Data.Interfaces;
        using System.Collections.Generic;
        using ViewModels.Watchlist;

        using static Common.ErrorMessages.Watchlist;

        [Authorize]
        public class WatchlistController : BaseController
        {
            private readonly IWatchlistService watchlistService;
            private readonly UserManager<ApplicationUser> userManager;

            public WatchlistController(IWatchlistService watchlistService, UserManager<ApplicationUser> userManager)
            {
                this.watchlistService = watchlistService;
                this.userManager = userManager;
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
                string userId = this.userManager.GetUserId(User)!;
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return this.RedirectToPage("/Identity/Account/Login");
                }

                IEnumerable<ApplicationUserWatchlistViewModel> watchList =
                    await this.watchlistService
                        .GetUserWatchListByUserIdAsync(userId);

                return View(watchList);
            }

            [HttpPost]
            public async Task<IActionResult> AddToWatchlist(string? movieId)
            {
                string userId = this.userManager.GetUserId(User)!;
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return this.RedirectToPage("/Identity/Account/Login");
                }

                if (String.IsNullOrWhiteSpace(movieId))
                {
                    TempData["ErrorMessage"] = "Invalid movie ID.";
                    return this.RedirectToAction("Index", "Movie");
                }

                bool result = await this.watchlistService.AddMovieToUserWatchListAsync(movieId, userId);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed to add movie to watchlist.";
                    return this.RedirectToAction("Index", "Movie");
                }

                TempData["SuccessMessage"] = "Movie added to watchlist successfully.";
                return this.RedirectToAction(nameof(Index));
            }


            [HttpPost]
            public async Task<IActionResult> RemoveFromWatchlist(string? movieId)
            {
                string userId = this.userManager.GetUserId(User)!;
                if (String.IsNullOrWhiteSpace(userId))
                {
                    return this.RedirectToPage("/Identity/Account/Login");
                }

                bool result = await this.watchlistService
                    .RemoveMovieFromUserWatchListAsync(movieId, userId);
                if (result == false)
                {
                    return this.RedirectToAction("Index", "Movie");
                }

                return this.RedirectToAction(nameof(Index));
            }

            [HttpGet]
            public async Task<IActionResult> IsInWatchlist(string? movieId)
            {
                string userId = this.userManager.GetUserId(User)!;
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(movieId))
                {
                    return Json(false);
                }

                bool isInWatchlist = await this.watchlistService
                    .IsMovieInUserWatchlistAsync(movieId, userId);

                return Json(isInWatchlist);
            }

        }
    }
}
