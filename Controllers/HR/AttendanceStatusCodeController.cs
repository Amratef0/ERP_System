using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.HR
{
    public class AttendanceStatusCodeController : Controller
    {
        private readonly IEmployeeTypeCodeService attendanceStatusCodeService;

        public AttendanceStatusCodeController(IEmployeeTypeCodeService attendanceStatusCodeService)
        {
            this.attendanceStatusCodeService = attendanceStatusCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<AttendanceStatusCode> attendanceStatusCodes = await attendanceStatusCodeService.GetAllAsync();
            return View("Index", attendanceStatusCodes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await attendanceStatusCodeService.CreateAsync(attendanceStatusCode);
                if (isCreated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Add", attendanceStatusCode);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var attendanceStatusCode = await attendanceStatusCodeService.GetByIdAsync(id);
            if (attendanceStatusCode == null)
                return NotFound();
            return View("Edit", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(AttendanceStatusCode attendanceStatusCode)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await attendanceStatusCodeService.UpdateAsync(attendanceStatusCode);
                if (isUpdated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await attendanceStatusCodeService.DeleteAsync(id);
            if (isDeleted)
                return RedirectToAction("Index");
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var attendanceStatusCode = await attendanceStatusCodeService.GetByIdAsync(id);
            if (attendanceStatusCode == null)
                return NotFound();
            return View("Details", attendanceStatusCode);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<AttendanceStatusCode> results = await attendanceStatusCodeService.SearchAsync(name);
            return Json(results);
        }
    }
}
