using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            this.leaveTypeService = leaveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<LeaveType> LeaveTypes = await leaveTypeService.GetAllAsync();
            return View("Index", LeaveTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(LeaveType LeaveType)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await leaveTypeService.CreateAsync(LeaveType);
                if (isCreated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Add", LeaveType);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var LeaveType = await leaveTypeService.GetByIdAsync(id);
            if (LeaveType == null)
                return NotFound();
            return View("Edit", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(LeaveType LeaveType)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await leaveTypeService.UpdateAsync(LeaveType);
                if (isUpdated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await leaveTypeService.DeleteAsync(id);
            if (isDeleted)
                return RedirectToAction("Index");
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var LeaveType = await leaveTypeService.GetByIdAsync(id);
            if (LeaveType == null)
                return NotFound();
            return View("Details", LeaveType);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<LeaveType> results = await leaveTypeService.SearchAsync(name);
            return Json(results);
        }
    }
}
