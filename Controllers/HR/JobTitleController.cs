using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                IEnumerable<JobTitle> JobTitles = await _jobTitleService.GetAllAsync();
                return View("Index", JobTitles);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading job titles. Please try again.";
                return View("Index", new List<JobTitle>());
            }
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
                try
                {
                    bool isCreated = await _jobTitleService.CreateAsync(JobTitle);
                    if (isCreated)
                    {
                        TempData["SuccessMessage"] = $"Job Title '{JobTitle.Name}' has been created successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to create job title.");
                    TempData["ErrorMessage"] = "Failed to create job title. Please try again.";
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A job title with the name '{JobTitle.Name}' already exists.");
                        TempData["ErrorMessage"] = "This job title name is already in use.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("Code"))
                    {
                        ModelState.AddModelError("Code", $"A job title with the code '{JobTitle.Code}' already exists.");
                        TempData["ErrorMessage"] = "This job title code is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create job title due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the job title.";
                }
            }
            
            return View("Create", JobTitle);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var JobTitle = await _jobTitleService.GetByIdAsync(id);
                if (JobTitle == null)
                {
                    TempData["ErrorMessage"] = "Job Title not found!";
                    return NotFound();
                }
                return View("Edit", JobTitle);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the job title.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobTitle JobTitle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = await _jobTitleService.UpdateAsync(JobTitle);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = $"Job Title '{JobTitle.Name}' has been updated successfully!";
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to update job title.");
                    TempData["ErrorMessage"] = "Failed to update job title. Please try again.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _jobTitleService.GetByIdAsync(JobTitle.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This job title has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This job title was modified by another user.");
                        TempData["WarningMessage"] = "The job title was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A job title with the name '{JobTitle.Name}' already exists.");
                        TempData["ErrorMessage"] = "This job title name is already in use.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("Code"))
                    {
                        ModelState.AddModelError("Code", $"A job title with the code '{JobTitle.Code}' already exists.");
                        TempData["ErrorMessage"] = "This job title code is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update job title due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the job title.";
                }
            }
            return View("Edit", JobTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
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
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete Job Title '{JobTitle.Name}'.";
                }
            }
            catch (DbUpdateException ex)
            {
                var JobTitle = await _jobTitleService.GetByIdAsync(id);
                var titleName = JobTitle?.Name ?? "this job title";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{titleName}' because it has associated employees. Please reassign employees to another job title first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{titleName}' due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the job title.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var JobTitle = await _jobTitleService.GetByIdAsync(id);
                if (JobTitle == null)
                {
                    TempData["ErrorMessage"] = "Job Title not found!";
                    return NotFound();
                }
                return View("Details", JobTitle);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading job title details.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<JobTitle> results = await _jobTitleService.SearchAsync(name);
                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search job titles." });
            }
        }
    }
}
