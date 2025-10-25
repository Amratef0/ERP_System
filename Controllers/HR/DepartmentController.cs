using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Department> departments = await _departmentService.GetAllAsync();
                return View("Index", departments);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading departments. Please try again.";
                return View("Index", new List<Department>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.CreateAsync(department);
                    TempData["SuccessMessage"] = $"Department '{department.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A department with the name '{department.Name}' already exists.");
                        TempData["ErrorMessage"] = "This department name is already in use. Please choose a different name.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Please check your input and try again.");
                        TempData["ErrorMessage"] = "Failed to create department due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the department.";
                }
            }

            return View("Create", department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null)
                {
                    TempData["ErrorMessage"] = "Department not found!";
                    return NotFound();
                }
                return View("Edit", department);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the department.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.UpdateAsync(department);
                    TempData["SuccessMessage"] = $"Department '{department.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _departmentService.GetByIdAsync(department.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This department has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This department was modified by another user. Please refresh and try again.");
                        TempData["WarningMessage"] = "The department was modified by another user. Please review the current data.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A department with the name '{department.Name}' already exists.");
                        TempData["ErrorMessage"] = "This department name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update department due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the department.";
                }
            }

            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var department = await _departmentService.GetByIdAsync(id);
                if (department == null)
                {
                    TempData["ErrorMessage"] = "Department not found!";
                    return NotFound();
                }
                
                await _departmentService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Department '{department.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                var department = await _departmentService.GetByIdAsync(id);
                var deptName = department?.Name ?? "this department";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{deptName}' because it has associated employees or records. Please reassign or remove them first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{deptName}' due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete this department: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the department.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Department department = await _departmentService.GetByIdAsync(id);
                if (department == null)
                {
                    TempData["ErrorMessage"] = "Department not found!";
                    return NotFound();
                }
                return View("Details", department);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading department details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<Department> departments = await _departmentService.SearchAsync(name);
                return Json(departments);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search departments." });
            }
        }
    }
}