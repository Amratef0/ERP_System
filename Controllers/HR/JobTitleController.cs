using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class JobTitleController : Controller
    {
        private readonly IJobTitleService jobTitleService;

        public JobTitleController(IJobTitleService jobTitleService)
        {
            this.jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<JobTitle> JobTitles = await jobTitleService.GetAllAsync();
            return View("Index", JobTitles);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(JobTitle JobTitle)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await jobTitleService.CreateAsync(JobTitle);
                if (isCreated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Add", JobTitle);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var JobTitle = await jobTitleService.GetByIdAsync(id);
            if (JobTitle == null)
                return NotFound();
            return View("Edit", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(JobTitle JobTitle)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await jobTitleService.UpdateAsync(JobTitle);
                if (isUpdated)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Something went wrong");
            }
            return View("Edit", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool isDeleted = await jobTitleService.DeleteAsync(id);
            if (isDeleted)
                return RedirectToAction("Index");
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var JobTitle = await jobTitleService.GetByIdAsync(id);
            if (JobTitle == null)
                return NotFound();
            return View("Details", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<JobTitle> results = await jobTitleService.SearchAsync(name);
            return Json(results);
        }
    }
}
