using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.ViewModels.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.Core
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Currency> currencies = await currencyService.GetAllAsync();
            return View("Index", currencies);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddAsync(Currency currency)
        {
            if (ModelState.IsValid)
            {
                await currencyService.CreateAsync(currency);
                return RedirectToAction("Index");
            }
            return View("Add", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await currencyService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Cannot delete this currency because it is referenced by other records.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            Currency currency = await currencyService.GetByIdAsync(id);
            if (currency == null) return NotFound();
            return View("Edit", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAsync(Currency currency)
        {
            if (ModelState.IsValid)
            {
                await currencyService.UpdateAsync(currency);
                return RedirectToAction("Index");
            }
            return View("Edit", currency);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<Currency> currencies = await currencyService.SearchByNameAsync(name);
            return Json(currencies);
        }
    }
}