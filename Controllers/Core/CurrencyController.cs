using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.ViewModels.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.Core
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Currency> currencies = await _currencyService.GetAllAsync();
                return View("Index", currencies);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading currencies. Please try again.";
                return View("Index", new List<Currency>());
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Currency currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _currencyService.CreateAsync(currency);
                    TempData["SuccessMessage"] = $"Currency '{currency.Name}' ({currency.Code}) has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError("Code", $"A currency with the code '{currency.Code}' already exists.");
                            TempData["ErrorMessage"] = "This currency code is already in use.";
                        }
                        else
                        {
                            ModelState.AddModelError("Name", $"A currency with the name '{currency.Name}' already exists.");
                            TempData["ErrorMessage"] = "This currency name is already in use.";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create currency due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the currency.";
                }
            }
            return View("Create", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var currency = await _currencyService.GetByIdAsync(id);
                if (currency == null)
                {
                    TempData["ErrorMessage"] = "Currency not found!";
                    return NotFound();
                }

                await _currencyService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Currency '{currency.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                var currency = await _currencyService.GetByIdAsync(id);
                var currencyName = currency?.Name ?? "this currency";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{currencyName}' because it is being used by employees, payroll entries, or other records. Please update these records first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{currencyName}' due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete this currency: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the currency.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                Currency currency = await _currencyService.GetByIdAsync(id);
                if (currency == null)
                {
                    TempData["ErrorMessage"] = "Currency not found!";
                    return NotFound();
                }
                return PartialView("Edit", currency);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the currency.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _currencyService.UpdateAsync(currency);
                    TempData["SuccessMessage"] = $"Currency '{currency.Name}' ({currency.Code}) has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _currencyService.GetByIdAsync(currency.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This currency has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This currency was modified by another user.");
                        TempData["WarningMessage"] = "The currency was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        if (ex.InnerException.Message.Contains("Code"))
                        {
                            ModelState.AddModelError("Code", $"A currency with the code '{currency.Code}' already exists.");
                            TempData["ErrorMessage"] = "This currency code is already in use.";
                        }
                        else
                        {
                            ModelState.AddModelError("Name", $"A currency with the name '{currency.Name}' already exists.");
                            TempData["ErrorMessage"] = "This currency name is already in use.";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update currency due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the currency.";
                }
            }
            return View("Edit", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<Currency> currencies = await _currencyService.SearchByNameAsync(name);
                return Json(currencies);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search currencies." });
            }
        }
    }
}