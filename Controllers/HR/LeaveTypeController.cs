using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<LeaveType> LeaveTypes = await _leaveTypeService.GetAllAsync();
                return View("Index", LeaveTypes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading leave types. Please try again.";
                return View("Index", new List<LeaveType>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveType LeaveType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isCreated = await _leaveTypeService.CreateAsync(LeaveType);
                    if (isCreated)
                    {
                        TempData["SuccessMessage"] = $"Leave Type '{LeaveType.Name}' has been created successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to create leave type.");
                    TempData["ErrorMessage"] = "Failed to create leave type. Please try again.";
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A leave type with the name '{LeaveType.Name}' already exists.");
                        TempData["ErrorMessage"] = "This leave type name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create leave type due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the leave type.";
                }
            }
            return View("Create", LeaveType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var LeaveType = await _leaveTypeService.GetByIdAsync(id);
                if (LeaveType == null)
                {
                    TempData["ErrorMessage"] = "Leave Type not found!";
                    return NotFound();
                }
                return View("Edit", LeaveType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the leave type.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveType LeaveType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = await _leaveTypeService.UpdateAsync(LeaveType);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = $"Leave Type '{LeaveType.Name}' has been updated successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to update leave type.");
                    TempData["ErrorMessage"] = "Failed to update leave type. Please try again.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _leaveTypeService.GetByIdAsync(LeaveType.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This leave type has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This leave type was modified by another user.");
                        TempData["WarningMessage"] = "The leave type was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A leave type with the name '{LeaveType.Name}' already exists.");
                        TempData["ErrorMessage"] = "This leave type name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update leave type due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the leave type.";
                }
            }
            return View("Edit", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var LeaveType = await _leaveTypeService.GetByIdAsync(id);
                if (LeaveType == null)
                {
                    TempData["ErrorMessage"] = "Leave Type not found!";
                    return NotFound();
                }

                bool isDeleted = await _leaveTypeService.DeleteAsync(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = $"Leave Type '{LeaveType.Name}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete Leave Type '{LeaveType.Name}'.";
                }
            }
            catch (DbUpdateException ex)
            {
                var LeaveType = await _leaveTypeService.GetByIdAsync(id);
                var typeName = LeaveType?.Name ?? "this leave type";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{typeName}' because it is being used by employee leave balances or leave requests. Please remove these associations first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{typeName}' due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the leave type.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var LeaveType = await _leaveTypeService.GetByIdAsync(id);
                if (LeaveType == null)
                {
                    TempData["ErrorMessage"] = "Leave Type not found!";
                    return NotFound();
                }
                return View("Details", LeaveType);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading leave type details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<LeaveType> results = await _leaveTypeService.SearchAsync(name);
                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search leave types." });
            }
        }
    }
}
