using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.HR
{
    public class AttendanceStatusCodeController : Controller
    {
        private readonly IEmployeeTypeCodeService _attendanceStatusCodeService;

        public AttendanceStatusCodeController(IEmployeeTypeCodeService attendanceStatusCodeService)
        {
            _attendanceStatusCodeService = attendanceStatusCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<AttendanceStatusCode> attendanceStatusCodes = await _attendanceStatusCodeService.GetAllAsync();
                return View("Index", attendanceStatusCodes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading attendance status codes. Please try again.";
                return View("Index", new List<AttendanceStatusCode>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isCreated = await _attendanceStatusCodeService.CreateAsync(attendanceStatusCode);
                    if (isCreated)
                    {
                        TempData["SuccessMessage"] = $"Attendance Status Code '{attendanceStatusCode.Name}' has been created successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to create attendance status code.");
                    TempData["ErrorMessage"] = "Failed to create attendance status code. Please try again.";
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError("Code", $"An attendance status code with the code '{attendanceStatusCode.Code}' already exists.");
                            TempData["ErrorMessage"] = "This attendance status code is already in use.";
                        }
                        else
                        {
                            ModelState.AddModelError("Name", $"An attendance status code with the name '{attendanceStatusCode.Name}' already exists.");
                            TempData["ErrorMessage"] = "This attendance status code name is already in use.";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create attendance status code due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the attendance status code.";
                }
            }
            return View("Create", attendanceStatusCode);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
                if (attendanceStatusCode == null)
                {
                    TempData["ErrorMessage"] = "Attendance Status Code not found!";
                    return NotFound();
                }
                return PartialView("Edit", attendanceStatusCode);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the attendance status code.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = await _attendanceStatusCodeService.UpdateAsync(attendanceStatusCode);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = $"Attendance Status Code '{attendanceStatusCode.Name}' has been updated successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to update attendance status code.");
                    TempData["ErrorMessage"] = "Failed to update attendance status code. Please try again.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _attendanceStatusCodeService.GetByIdAsync(attendanceStatusCode.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This attendance status code has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This attendance status code was modified by another user.");
                        TempData["WarningMessage"] = "The attendance status code was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError("Code", $"An attendance status code with the code '{attendanceStatusCode.Code}' already exists.");
                            TempData["ErrorMessage"] = "This attendance status code is already in use.";
                        }
                        else
                        {
                            ModelState.AddModelError("Name", $"An attendance status code with the name '{attendanceStatusCode.Name}' already exists.");
                            TempData["ErrorMessage"] = "This attendance status code name is already in use.";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update attendance status code due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the attendance status code.";
                }
            }
            return View("Edit", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
                if (attendanceStatusCode == null)
                {
                    TempData["ErrorMessage"] = "Attendance Status Code not found!";
                    return NotFound();
                }

                bool isDeleted = await _attendanceStatusCodeService.DeleteAsync(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = $"Attendance Status Code '{attendanceStatusCode.Name}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete Attendance Status Code '{attendanceStatusCode.Name}'.";
                }
            }
            catch (DbUpdateException ex)
            {
                var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
                var codeName = attendanceStatusCode?.Name ?? "this attendance status code";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{codeName}' because it is being used by attendance records. Please remove these associations first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{codeName}' due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the attendance status code.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
                if (attendanceStatusCode == null)
                {
                    TempData["ErrorMessage"] = "Attendance Status Code not found!";
                    return NotFound();
                }
                return PartialView("Details", attendanceStatusCode);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading attendance status code details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<AttendanceStatusCode> results = await _attendanceStatusCodeService.SearchAsync(name);
                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search attendance status codes." });
            }
        }
    }
}
