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
            IEnumerable<Currency> currencies = await _currencyService.GetAllAsync();
            return View("Index", currencies);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Currency currency)
        {
            if (ModelState.IsValid)
            {
                await _currencyService.CreateAsync(currency);
                TempData["SuccessMessage"] = $"Currency '{currency.Name}' ({currency.Code}) has been created successfully!";
                return RedirectToAction("Index");
            }
            return View("Create", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currency = await _currencyService.GetByIdAsync(id);
            if (currency == null)
            {
                TempData["ErrorMessage"] = "Currency not found!";
                return NotFound();
            }

            try
            {
                await _currencyService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Currency '{currency.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");

                TempData["ErrorMessage"] = $"Cannot delete currency '{currency.Name}' ({currency.Code}) because it is being used by other records (employees, salary configurations, etc.).";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete currency '{currency.Name}': {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting currency '{currency.Name}': {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Currency currency = await _currencyService.GetByIdAsync(id);
            if (currency == null)
            {
                TempData["ErrorMessage"] = "Currency not found!";
                return NotFound();
            }
            return View("Edit", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                await _currencyService.UpdateAsync(currency);
                TempData["SuccessMessage"] = $"Currency '{currency.Name}' ({currency.Code}) has been updated successfully!";
                return RedirectToAction("Index");
            }
            return View("Edit", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<Currency> currencies = await _currencyService.SearchByNameAsync(name);
            return Json(currencies);
        }
    }
}