using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ERP_System_Project.Controllers.Core
{
    public class CountryController : Controller
    {
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Country> countries = await countryService.GetAllAsync();
            return View("Index", countries);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(Country country)
        {
            if (ModelState.IsValid)
            {
                await countryService.CreateAsync(country);
                return RedirectToAction("Index");
            }

            return View("Add", country);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            Country country = await countryService.GetByIdAsync(id);
            return View("Edit", country);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(Country country)
        {
            if (ModelState.IsValid)
            {
                await countryService.UpdateAsync(country);
                return RedirectToAction("Index");
            }

            return View("Edit", country);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await countryService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Cannot delete this country because it is referenced by other records.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SearchAsync(string name)
        {
            IEnumerable<Country> filteredCountries = await countryService.SearchByNameAsync(name);
            return Json(filteredCountries);
        }
    }
}