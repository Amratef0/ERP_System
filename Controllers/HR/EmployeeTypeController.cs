using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class EmployeeTypeController : Controller
    {
        private readonly IEmployeeTypeService employeeTypeService;

        public EmployeeTypeController(IEmployeeTypeService employeeTypeService)
        {
            this.employeeTypeService = employeeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<EmployeeType> employeeTypes = await employeeTypeService.GetAllAsync();
            return View("Index", employeeTypes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await employeeTypeService.CreateAsync(employeeType);
                if (isCreated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Add", employeeType);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var employeeType = await employeeTypeService.GetByIdAsync(id);
            if (employeeType == null)
                return NotFound();
            return View("Edit", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await employeeTypeService.UpdateAsync(employeeType);
                if (isUpdated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await employeeTypeService.DeleteAsync(id);
            if (isDeleted)
                return RedirectToAction("Index");
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var employeeType = await employeeTypeService.GetByIdAsync(id);
            if (employeeType == null)
                return NotFound();
            return View("Details", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<EmployeeType> results = await employeeTypeService.SearchAsync(name);
            return Json(results);
        }
    }
}
