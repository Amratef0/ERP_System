using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class EmployeeTypeController : Controller
    {
        private readonly IEmployeeTypeService _employeeTypeService;

        public EmployeeTypeController(IEmployeeTypeService employeeTypeService)
        {
            _employeeTypeService = employeeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<EmployeeType> employeeTypes = await _employeeTypeService.GetAllAsync();
                return View("Index", employeeTypes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading employee types. Please try again.";
                return View("Index", new List<EmployeeType>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isCreated = await _employeeTypeService.CreateAsync(employeeType);
                    if (isCreated)
                    {
                        TempData["SuccessMessage"] = $"Employee Type '{employeeType.Name}' has been created successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to create employee type.");
                    TempData["ErrorMessage"] = "Failed to create employee type. Please try again.";
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"An employee type with the name '{employeeType.Name}' already exists.");
                        TempData["ErrorMessage"] = "This employee type name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create employee type due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the employee type.";
                }
            }
            return View("Create", employeeType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var employeeType = await _employeeTypeService.GetByIdAsync(id);
                if (employeeType == null)
                {
                    TempData["ErrorMessage"] = "Employee Type not found!";
                    return NotFound();
                }
                return PartialView("Edit", employeeType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the employee type.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = await _employeeTypeService.UpdateAsync(employeeType);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = $"Employee Type '{employeeType.Name}' has been updated successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to update employee type.");
                    TempData["ErrorMessage"] = "Failed to update employee type. Please try again.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _employeeTypeService.GetByIdAsync(employeeType.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This employee type has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This employee type was modified by another user.");
                        TempData["WarningMessage"] = "The employee type was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"An employee type with the name '{employeeType.Name}' already exists.");
                        TempData["ErrorMessage"] = "This employee type name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update employee type due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the employee type.";
                }
            }
            return View("Edit", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employeeType = await _employeeTypeService.GetByIdAsync(id);
                if (employeeType == null)
                {
                    TempData["ErrorMessage"] = "Employee Type not found!";
                    return NotFound();
                }

                bool isDeleted = await _employeeTypeService.DeleteAsync(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = $"Employee Type '{employeeType.Name}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete Employee Type '{employeeType.Name}'.";
                }
            }
            catch (DbUpdateException ex)
            {
                var employeeType = await _employeeTypeService.GetByIdAsync(id);
                var typeName = employeeType?.Name ?? "this employee type";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{typeName}' because it has associated employees. Please reassign employees to another type first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{typeName}' due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the employee type.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var employeeType = await _employeeTypeService.GetByIdAsync(id);
                if (employeeType == null)
                {
                    TempData["ErrorMessage"] = "Employee Type not found!";
                    return NotFound();
                }
                return PartialView("Details", employeeType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading employee type details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<EmployeeType> results = await _employeeTypeService.SearchAsync(name);
                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search employee types." });
            }
        }
    }
}
