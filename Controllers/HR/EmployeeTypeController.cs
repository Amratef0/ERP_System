using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

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
            IEnumerable<EmployeeType> employeeTypes = await _employeeTypeService.GetAllAsync();
            return View("Index", employeeTypes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await _employeeTypeService.CreateAsync(employeeType);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = $"Employee Type '{employeeType.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Create", employeeType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employeeType = await _employeeTypeService.GetByIdAsync(id);
            if (employeeType == null)
            {
                TempData["ErrorMessage"] = "Employee Type not found!";
                return NotFound();
            }
            return View("Edit", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await _employeeTypeService.UpdateAsync(employeeType);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Employee Type '{employeeType.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Failed to delete Employee Type!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employeeType = await _employeeTypeService.GetByIdAsync(id);
            if (employeeType == null)
                return NotFound();
            return View("Details", employeeType);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<EmployeeType> results = await _employeeTypeService.SearchAsync(name);
            return Json(results);
        }
    }
}
