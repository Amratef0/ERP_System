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
            try
            {
                IEnumerable<Country> countries = await _countryService.GetAllAsync();
                return View("Index", countries);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading countries. Please try again.";
                return View("Index", new List<Country>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _countryService.CreateAsync(country);
                    TempData["SuccessMessage"] = $"Country '{country.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A country with the name '{country.Name}' already exists.");
                        TempData["ErrorMessage"] = "This country name is already in use.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("Code"))
                    {
                        ModelState.AddModelError("Code", $"A country with the code '{country.Code}' already exists.");
                        TempData["ErrorMessage"] = "This country code is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create country due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the country.";
                }
            }

            return View("Create", country);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            try
            {
                Country country = await _countryService.GetByIdAsync(id);
                if (country == null)
                {
                    TempData["ErrorMessage"] = "Country not found!";
                    return NotFound();
                }
                return PartialView("Edit", country);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the country.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _countryService.UpdateAsync(country);
                    TempData["SuccessMessage"] = $"Country '{country.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _countryService.GetByIdAsync(country.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This country has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This country was modified by another user.");
                        TempData["WarningMessage"] = "The country was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A country with the name '{country.Name}' already exists.");
                        TempData["ErrorMessage"] = "This country name is already in use.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("Code"))
                    {
                        ModelState.AddModelError("Code", $"A country with the code '{country.Code}' already exists.");
                        TempData["ErrorMessage"] = "This country code is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update country due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the country.";
                }
            }

            return View("Edit", country);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var country = await _countryService.GetByIdAsync(id);
                if (country == null)
                {
                    TempData["ErrorMessage"] = "Country not found!";
                    return NotFound();
                }

                await _countryService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Country '{country.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                var country = await _countryService.GetByIdAsync(id);
                var countryName = country?.Name ?? "this country";

                if (ex.InnerException != null &&
                    (ex.InnerException.Message.Contains("REFERENCE constraint") ||
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{countryName}' because it is being used by addresses, branches, public holidays, or employees. Please remove these associations first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{countryName}' due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete this country: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the country.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(string name)
        {
            try
            {
                IEnumerable<Country> filteredCountries = await _countryService.SearchByNameAsync(name);
                return Json(filteredCountries);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search countries." });
            }
        }
    }
}