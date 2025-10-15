using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class JobTitleController : Controller
    {
        private readonly IJobTitleService _jobTitleService;

        public JobTitleController(IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<JobTitle> JobTitles = await _jobTitleService.GetAllAsync();
            return View("Index", JobTitles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobTitle JobTitle)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await _jobTitleService.CreateAsync(JobTitle);
                if (isCreated)
                {
                    TempData["SuccessMessage"] = $"Job Title '{JobTitle.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Create", JobTitle);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var JobTitle = await _jobTitleService.GetByIdAsync(id);
            if (JobTitle == null)
            {
                TempData["ErrorMessage"] = "Job Title not found!";
                return NotFound();
            }
            return View("Edit", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobTitle JobTitle)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await _jobTitleService.UpdateAsync(JobTitle);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Job Title '{JobTitle.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var JobTitle = await _jobTitleService.GetByIdAsync(id);
            if (JobTitle == null)
            {
                TempData["ErrorMessage"] = "Job Title not found!";
                return NotFound();
            }

            bool isDeleted = await _jobTitleService.DeleteAsync(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = $"Job Title '{JobTitle.Name}' has been deleted successfully!";
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = "Failed to delete Job Title!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var JobTitle = await _jobTitleService.GetByIdAsync(id);
            if (JobTitle == null)
                return NotFound();
            return View("Details", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<JobTitle> results = await _jobTitleService.SearchAsync(name);
            return Json(results);
        }
    }
}
