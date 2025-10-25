using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class PayrollEntryController : Controller
    {
        private readonly IPayrollEntryService _payrollEntryService;
        private readonly IPayrollRunService _payrollRunService;

        public PayrollEntryController(
            IPayrollEntryService payrollEntryService,
            IPayrollRunService payrollRunService)
        {
            _payrollEntryService = payrollEntryService;
            _payrollRunService = payrollRunService;
        }

        // GET: PayrollEntry/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var entry = await _payrollEntryService.GetPayrollEntryDetailsAsync(id);

                if (entry == null)
                {
                    TempData["ErrorMessage"] = "Payroll entry not found.";
                    return RedirectToAction("Index", "PayrollRun");
                }

                return View(entry);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the payroll entry.";
                return RedirectToAction("Index", "PayrollRun");
            }
        }

        // GET: PayrollEntry/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var entry = await _payrollEntryService.GetPayrollEntryDetailsAsync(id);

                if (entry == null)
                {
                    TempData["ErrorMessage"] = "Payroll entry not found.";
                    return RedirectToAction("Index", "PayrollRun");
                }

                if (entry.IsLocked)
                {
                    TempData["ErrorMessage"] = "Cannot edit entry in a locked payroll run.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                return View(entry);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the payroll entry.";
                return RedirectToAction("Index", "PayrollRun");
            }
        }

        // POST: PayrollEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PayrollEntryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _payrollEntryService.UpdatePayrollEntryAsync(model);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Payroll entry updated successfully!";
                    return RedirectToAction("Details", "PayrollRun", new { id = model.PayrollRunId });
                }

                ModelState.AddModelError("", result.ErrorMessage ?? "Failed to update payroll entry.");
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to update payroll entry.";
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "This payroll entry was modified by another user.");
                TempData["WarningMessage"] = "The payroll entry was modified by another user. Please refresh and try again.";
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("CHECK constraint"))
                {
                    ModelState.AddModelError("", "Invalid amount values. Please check your calculations.");
                    TempData["ErrorMessage"] = "Invalid amount values. Net amount cannot be negative.";
                }
                else if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("FOREIGN KEY"))
                {
                    ModelState.AddModelError("", "Invalid employee or currency reference.");
                    TempData["ErrorMessage"] = "Invalid employee or currency selected.";
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                    TempData["ErrorMessage"] = "Failed to update payroll entry due to a database error.";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                TempData["ErrorMessage"] = "An unexpected error occurred while updating the payroll entry.";
            }

            return View(model);
        }

        // POST: PayrollEntry/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int payrollRunId)
        {
            try
            {
                var result = await _payrollEntryService.DeletePayrollEntryAsync(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Payroll entry deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to delete payroll entry.";
                }
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && 
                    (dbEx.InnerException.Message.Contains("REFERENCE constraint") || 
                     dbEx.InnerException.Message.Contains("FOREIGN KEY constraint")))
                {
                    TempData["ErrorMessage"] = "Cannot delete this payroll entry because it is being referenced by other records.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete payroll entry due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete this payroll entry: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the payroll entry.";
            }

            return RedirectToAction("Details", "PayrollRun", new { id = payrollRunId });
        }
    }
}
