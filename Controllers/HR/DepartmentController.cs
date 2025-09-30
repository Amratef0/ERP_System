using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Department> departments = await departmentService.GetAllAsync();
            return View("Index", departments);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.CreateAsync(department);
                return RedirectToAction("Index");
            }

            return View("Add", department);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            Department department = await departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();
            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(Department department)
        {
            if (ModelState.IsValid)
            {
                await departmentService.UpdateAsync(department);
                return RedirectToAction("Index");
            }

            return View("Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await departmentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            Department department = await departmentService.GetByIdAsync(id);
            if (department == null) return NotFound();
            return View("Details", department);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<Department> departments = await departmentService.SearchAsync(name);
            return Json(departments);
        }
    }
}