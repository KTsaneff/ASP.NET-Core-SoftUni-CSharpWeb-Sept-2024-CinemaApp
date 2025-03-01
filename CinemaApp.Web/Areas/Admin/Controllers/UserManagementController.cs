using CinemaApp.Data.Models;
using CinemaApp.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(Guid userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null || !await roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Invalid user or role.");
            }

            if (await userManager.IsInRoleAsync(user, role))
            {
                TempData["ErrorMessage"] = "User already has this role.";
                return RedirectToAction(nameof(Index));
            }

            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("Cannot modify Admin roles.");
            }

            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Failed to assign role.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Role '{role}' assigned successfully to {user.Email}.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(Guid userId, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                TempData["ErrorMessage"] = "Role selection is required.";
                return RedirectToAction(nameof(Index));
            }

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            bool roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                TempData["ErrorMessage"] = "Invalid role selected.";
                return RedirectToAction(nameof(Index));
            }

            await userManager.RemoveFromRoleAsync(user, role);
            TempData["SuccessMessage"] = $"Role '{role}' removed successfully from {user.Email}.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("Cannot delete Admin users.");
            }

            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
