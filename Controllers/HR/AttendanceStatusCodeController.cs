using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<AttendanceStatusCode> attendanceStatusCodes = await _attendanceStatusCodeService.GetAllAsync();
            return View("Index", attendanceStatusCodes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await _attendanceStatusCodeService.CreateAsync(attendanceStatusCode);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = $"Attendance Status Code '{attendanceStatusCode.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Create", attendanceStatusCode);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
            if (attendanceStatusCode == null)
            {
                TempData["ErrorMessage"] = "Attendance Status Code not found!";
                return NotFound();
            }
            return View("Edit", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await _attendanceStatusCodeService.UpdateAsync(attendanceStatusCode);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Attendance Status Code '{attendanceStatusCode.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Failed to delete Attendance Status Code!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var attendanceStatusCode = await _attendanceStatusCodeService.GetByIdAsync(id);
            if (attendanceStatusCode == null)
                return NotFound();
            return View("Details", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<AttendanceStatusCode> results = await _attendanceStatusCodeService.SearchAsync(name);
            return Json(results);
        }
    }
}
