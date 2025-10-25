using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class PublicHolidayController : Controller
    {
        private readonly IPublicHolidayService _publicHolidayService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly IValidator<PublicHolidayCountriesVM> _validator;

        public PublicHolidayController(IPublicHolidayService publicHolidayService, ICountryService countryService, IMapper mapper, IValidator<PublicHolidayCountriesVM> validator)
        {
            _publicHolidayService = publicHolidayService;
            _countryService = countryService;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                PublicHolidayIndexVM model = new PublicHolidayIndexVM
                {
                    PublicHolidays = await _publicHolidayService.GetAllWithCountryAsync(),
                    Countries = await _countryService.GetAllAsync()
                };
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading public holidays. Please try again.";
                return View("Index", new PublicHolidayIndexVM());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                PublicHolidayCountriesVM vm = new PublicHolidayCountriesVM
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Countries = await _countryService.GetAllAsync()
                };
                return View("Create", vm);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(PublicHolidayCountriesVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    PublicHoliday publicHoliday = _mapper.Map<PublicHoliday>(model);
                    await _publicHolidayService.CreateAsync(publicHoliday);
                    TempData["SuccessMessage"] = $"Public Holiday '{model.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("", "A public holiday with the same name and date already exists for this country.");
                        TempData["ErrorMessage"] = "This public holiday already exists for the selected country.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    {
                        ModelState.AddModelError("", "A public holiday with these details already exists.");
                        TempData["ErrorMessage"] = "This public holiday already exists.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create public holiday due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the public holiday.";
                }
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            model.Countries = await _countryService.GetAllAsync();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                PublicHoliday publicHoliday = await _publicHolidayService.GetByIdAsync(id);
                if (publicHoliday == null)
                {
                    TempData["ErrorMessage"] = "Public Holiday not found!";
                    return NotFound();
                }

                PublicHolidayCountriesVM vm = _mapper.Map<PublicHolidayCountriesVM>(publicHoliday);
                vm.Countries = await _countryService.GetAllAsync();

                return View("Edit", vm);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the public holiday.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PublicHolidayCountriesVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    PublicHoliday publicHoliday = _mapper.Map<PublicHoliday>(model);
                    await _publicHolidayService.UpdateAsync(publicHoliday);
                    TempData["SuccessMessage"] = $"Public Holiday '{model.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _publicHolidayService.GetByIdAsync(model.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This public holiday has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This public holiday was modified by another user.");
                        TempData["WarningMessage"] = "The public holiday was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("", "A public holiday with the same name and date already exists for this country.");
                        TempData["ErrorMessage"] = "This public holiday already exists for the selected country.";
                    }
                    else if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    {
                        ModelState.AddModelError("", "A public holiday with these details already exists.");
                        TempData["ErrorMessage"] = "This public holiday already exists.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update public holiday due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the public holiday.";
                }
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            model.Countries = await _countryService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var publicHoliday = await _publicHolidayService.GetByIdAsync(id);
                if (publicHoliday == null)
                {
                    TempData["ErrorMessage"] = "Public Holiday not found!";
                    return NotFound();
                }

                await _publicHolidayService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Public Holiday '{publicHoliday.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                var publicHoliday = await _publicHolidayService.GetByIdAsync(id);
                var holidayName = publicHoliday?.Name ?? "this public holiday";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{holidayName}' because it is being referenced by other records. Please remove these associations first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{holidayName}' due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the public holiday.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(string name, int countryId)
        {
            try
            {
                IEnumerable<PublicHoliday>? filteredPublicHoliday = await _publicHolidayService.FilterAsync(name, countryId);
                return Json(filteredPublicHoliday ?? new List<PublicHoliday>());
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to filter public holidays." });
            }
        }
    }
}