using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class PublicHolidayController : Controller
    {
        private readonly IPublicHolidayService publicHolidayService;
        private readonly ICountryService countryService;
        private readonly IMapper mapper;
        private readonly IValidator<PublicHolidayCountriesVM> validator;

        public PublicHolidayController(IPublicHolidayService publicHolidayService, ICountryService countryService, IMapper mapper, IValidator<PublicHolidayCountriesVM> validator)
        {
            this.publicHolidayService = publicHolidayService;
            this.countryService = countryService;
            this.mapper = mapper;
            this.validator = validator;
        }


        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            List<PublicHoliday> publicHolidays = await publicHolidayService.GetAllAsync();
            return View("Index", publicHolidays);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            PublicHolidayCountriesVM vm = new PublicHolidayCountriesVM
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Countries = await countryService.GetAllAsync()
            };
            return View("Add", vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddAsync(PublicHolidayCountriesVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                PublicHoliday publicHoliday = mapper.Map<PublicHoliday>(model);
                await publicHolidayService.CreateAsync(publicHoliday);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await countryService.GetAllAsync();
            return View("Add", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            PublicHoliday publicHoliday = await publicHolidayService.GetByIdAsync(id);
            if (publicHoliday == null) return NotFound();

            PublicHolidayCountriesVM vm = mapper.Map<PublicHolidayCountriesVM>(publicHoliday);
            vm.Countries = await countryService.GetAllAsync();

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(PublicHolidayCountriesVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                PublicHoliday publicHoliday = mapper.Map<PublicHoliday>(model);
                await publicHolidayService.UpdateAsync(publicHoliday);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await countryService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await publicHolidayService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
