using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

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
            IEnumerable<LeaveType> LeaveTypes = await _leaveTypeService.GetAllAsync();
            return View("Index", LeaveTypes);
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
                bool isCreated = await _leaveTypeService.CreateAsync(LeaveType);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = $"Leave Type '{LeaveType.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Create", LeaveType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var LeaveType = await _leaveTypeService.GetByIdAsync(id);
            if (LeaveType == null)
            {
                TempData["ErrorMessage"] = "Leave Type not found!";
                return NotFound();
            }
            return View("Edit", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveType LeaveType)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await _leaveTypeService.UpdateAsync(LeaveType);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Leave Type '{LeaveType.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Failed to delete Leave Type!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var LeaveType = await _leaveTypeService.GetByIdAsync(id);
            if (LeaveType == null)
                return NotFound();
            return View("Details", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<LeaveType> results = await _leaveTypeService.SearchAsync(name);
            return Json(results);
        }
    }
}
