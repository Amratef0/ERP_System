using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERP_System_Project.Controllers.Core
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Country> countries = await _countryService.GetAllAsync();
            return View("Index", countries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryService.CreateAsync(country);
                TempData["SuccessMessage"] = $"Country '{country.Name}' has been created successfully!";
                return RedirectToAction("Index");
            }

            return View("Create", country);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            Country country = await _countryService.GetByIdAsync(id);
            if (country == null)
            {
                TempData["ErrorMessage"] = "Country not found!";
                return NotFound();
            }
            return View("Edit", country);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                await _countryService.UpdateAsync(country);
                TempData["SuccessMessage"] = $"Country '{country.Name}' has been updated successfully!";
                return RedirectToAction("Index");
            }

            return View("Edit", country);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _countryService.GetByIdAsync(id);
            if (country == null)
            {
                TempData["ErrorMessage"] = "Country not found!";
                return NotFound();
            }

            try
            {
                await _countryService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Country '{country.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");


                TempData["ErrorMessage"] = $"Cannot delete country '{country.Name}' because it is being used by other records (addresses, branches, public holidays, or employees).";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete country '{country.Name}': {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting country '{country.Name}': {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(string name)
        {
            IEnumerable<Country> filteredCountries = await _countryService.SearchByNameAsync(name);
            return Json(filteredCountries);
        }
    }
}