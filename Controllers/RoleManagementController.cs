using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ERP_System_Project.Models.Authentication;
using System.Security.Claims;

namespace ERP_System_Project.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// View to assign roles to current user (Development/Testing Only)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            ViewBag.UserEmail = user.Email;
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = allRoles;

            return View();
        }

        /// <summary>
        /// Assign a role to current user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string roleName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(AssignRole));
            }

            // Check if role exists
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                // Create the role if it doesn't exist
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                TempData["Success"] = $"Role '{roleName}' created successfully!";
            }

            // Check if user already has this role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                TempData["Warning"] = $"You already have the '{roleName}' role.";
                return RedirectToAction(nameof(AssignRole));
            }

            // Assign the role
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{roleName}' assigned successfully! Please logout and login again to see changes.";
            }
            else
            {
                TempData["Error"] = $"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return RedirectToAction(nameof(AssignRole));
        }

        /// <summary>
        /// Remove a role from current user
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(AssignRole));
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{roleName}' removed successfully! Please logout and login again to see changes.";
            }
            else
            {
                TempData["Error"] = $"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return RedirectToAction(nameof(AssignRole));
        }

        /// <summary>
        /// Create all standard roles
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStandardRoles()
        {
            var roles = new[] { "Admin", "HR Manager", "Manager", "Employee" };
            var createdRoles = new List<string>();

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    createdRoles.Add(role);
                }
            }

            if (createdRoles.Any())
            {
                TempData["Success"] = $"Created roles: {string.Join(", ", createdRoles)}";
            }
            else
            {
                TempData["Info"] = "All standard roles already exist.";
            }

            return RedirectToAction(nameof(AssignRole));
        }
    }
}
