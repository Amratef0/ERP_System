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
            IEnumerable<Department> departments = await _departmentService.GetAllAsync();
            return View("Index", departments);
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
                await _departmentService.CreateAsync(department);
                TempData["SuccessMessage"] = $"Department '{department.Name}' has been created successfully!";
                return RedirectToAction("Index");
            }

            return View("Create", department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Department department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                TempData["ErrorMessage"] = "Department not found!";
                return NotFound();
            }
            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateAsync(department);
                TempData["SuccessMessage"] = $"Department '{department.Name}' has been updated successfully!";
                return RedirectToAction("Index");
            }

            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                TempData["ErrorMessage"] = "Department not found!";
                return NotFound();
            }
            
            try
            {
                await _departmentService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Department '{department.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                
                // Check if it's a foreign key constraint violation
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete department '{department.Name}' because it is being used by employees or other records.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete department '{department.Name}': {ex.InnerException?.Message ?? ex.Message}";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete department '{department.Name}': {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting department '{department.Name}': {ex.Message}";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Department department = await _departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();
            return View("Details", department);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<Department> departments = await _departmentService.SearchAsync(name);
            return Json(departments);
        }
    }
}