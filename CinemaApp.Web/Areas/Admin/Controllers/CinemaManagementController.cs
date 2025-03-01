namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using CinemaApp.Web.ViewModels.Cinema;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CinemaManagementController : Controller
    {
        private readonly ICinemaService cinemaService;

        public CinemaManagementController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<CinemaIndexViewModel> cinemas =
                await this.cinemaService.GetAllCinemasForAdminAsync();

            return this.View(cinemas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View(new AddCinemaFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCinemaFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.cinemaService.AddCinemaAsync(model);

            TempData["SuccessMessage"] = $"Cinema '{model.Name}' added successfully!";
            return this.RedirectToAction(nameof(Manage));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var cinema = await this.cinemaService.GetCinemaForEditByIdAsync(id);

            if (cinema == null)
            {
                TempData["ErrorMessage"] = "Cinema not found.";
                return this.RedirectToAction(nameof(Manage));
            }

            return this.View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCinemaFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            bool isUpdated = await this.cinemaService.EditCinemaAsync(model);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Failed to update the cinema.";
                return this.View(model);
            }

            TempData["SuccessMessage"] = $"Cinema '{model.Name}' updated successfully!";
            return this.RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDelete(Guid id)
        {
            bool result = await this.cinemaService.ToggleDeleteCinemaAsync(id);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to delete cinema.";
            }
            else
            {
                TempData["SuccessMessage"] = "Cinema deleted successfully!";
            }

            return this.RedirectToAction(nameof(Manage));
        }
    }
}
