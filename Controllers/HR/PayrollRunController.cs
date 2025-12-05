using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class PayrollRunController : Controller
    {
        private readonly IPayrollRunService _payrollRunService;
        private readonly IPayrollEntryService _payrollEntryService;
        private readonly ICurrencyService _currencyService;

        public PayrollRunController(
            IPayrollRunService payrollRunService,
            IPayrollEntryService payrollEntryService,
            ICurrencyService currencyService)
        {
            _payrollRunService = payrollRunService;
            _payrollEntryService = payrollEntryService;
            _currencyService = currencyService;
        }

        // GET: PayrollRun/Index
        [HttpGet]
        public async Task<IActionResult> Index(int? year, int? month, bool? isLocked)
        {
            try
            {
                var payrollRuns = await _payrollRunService.GetAllPayrollRunsAsync(year, month, isLocked);
                
                ViewBag.SelectedYear = year;
                ViewBag.SelectedMonth = month;
                ViewBag.SelectedIsLocked = isLocked;
                
                return View(payrollRuns);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading payroll runs. Please try again.";
                return View(new List<PayrollRunVM>());
            }
        }

        // GET: PayrollRun/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var payrollRun = await _payrollRunService.GetPayrollRunDetailsAsync(id);
                
                if (payrollRun == null)
                {
                    TempData["ErrorMessage"] = "Payroll run not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(payrollRun);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the payroll run.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PayrollRun/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreatePayrollRunVM
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PayrollRun/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePayrollRunVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _payrollRunService.GeneratePayrollRunAsync(model.Year, model.Month);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ErrorMessage ?? "Payroll run created successfully!";
                    return RedirectToAction(nameof(Details), new { id = result.PayrollRunId });
                }

                ModelState.AddModelError("", result.ErrorMessage ?? "Failed to create payroll run.");
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to create payroll run.";
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("duplicate key"))
                {
                    ModelState.AddModelError("", "A payroll run already exists for this period.");
                    TempData["ErrorMessage"] = "A payroll run already exists for this month and year.";
                }
                else if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("UNIQUE"))
                {
                    ModelState.AddModelError("", "A payroll run with this name already exists.");
                    TempData["ErrorMessage"] = "A payroll run with this name already exists.";
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                    TempData["ErrorMessage"] = "Failed to create payroll run due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the payroll run.";
            }

            return View(model);
        }

        // POST: PayrollRun/Regenerate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regenerate(int id)
        {
            try
            {
                var result = await _payrollRunService.RegeneratePayrollRunAsync(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ErrorMessage ?? "Payroll run regenerated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to regenerate payroll run.";
                }
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = "Failed to regenerate payroll run due to a database error.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot regenerate payroll run: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while regenerating the payroll run.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: PayrollRun/Lock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(int id)
        {
            try
            {
                var result = await _payrollRunService.LockPayrollRunAsync(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ErrorMessage ?? "Payroll run confirmed and locked successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to lock payroll run.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["WarningMessage"] = "The payroll run was modified by another user. Please refresh and try again.";
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = "Failed to lock payroll run due to a database error.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot lock payroll run: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while locking the payroll run.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: PayrollRun/Unlock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(int id)
        {
            try
            {
                var result = await _payrollRunService.UnlockPayrollRunAsync(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ErrorMessage ?? "Payroll run unlocked successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to unlock payroll run.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["WarningMessage"] = "The payroll run was modified by another user. Please refresh and try again.";
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = "Failed to unlock payroll run due to a database error.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot unlock payroll run: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while unlocking the payroll run.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: PayrollRun/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Delete all associated payroll entries first, then delete the payroll run
                var result = await _payrollRunService.DeletePayrollRunWithEntriesAsync(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ErrorMessage ?? "Payroll run and all associated entries deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to delete payroll run.";
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && 
                    (dbEx.InnerException.Message.Contains("REFERENCE constraint") || 
                     dbEx.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     dbEx.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = "Cannot delete this payroll run due to database constraints. The payroll run may be locked or referenced by other records.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete payroll run due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete payroll run: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the payroll run.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
