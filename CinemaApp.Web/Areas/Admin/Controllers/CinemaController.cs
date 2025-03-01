namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using CinemaApp.Web.ViewModels.Cinema;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaService cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<CinemaIndexViewModel> cinemas =
                await this.cinemaService.IndexGetAllOrderedByLocationAsync();

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
            EditCinemaFormModel? formModel = await this.cinemaService.GetCinemaForEditByIdAsync(id);
            if (formModel == null)
            {
                return this.NotFound();
            }

            return this.View(formModel);
        }

        // POST: Update Cinema Details
        [HttpPost]
        public async Task<IActionResult> Edit(EditCinemaFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }

            bool isUpdated = await this.cinemaService.EditCinemaAsync(formModel);
            if (!isUpdated)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while updating the cinema.");
                return this.View(formModel);
            }

            return this.RedirectToAction(nameof(Manage));
        }

        // GET: Confirm Deletion of a Cinema
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            //CinemaIndexViewModel? cinema = await this.cinemaService.GetCinemaByIdAsync(id);
            //if (cinema == null)
            //{
            //    return this.NotFound();
            //}

            return this.View(/*cinema*/);
        }

        // POST: Delete a Cinema
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //bool isDeleted = await this.cinemaService.DeleteCinemaAsync(id);
            //if (!isDeleted)
            //{
            //    ModelState.AddModelError(string.Empty, "Failed to delete the cinema.");
            //    return this.View();
            //}

            return this.RedirectToAction(nameof(Manage));
        }
    }
}
