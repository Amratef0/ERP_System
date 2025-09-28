using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class WorkScheduleController : Controller
    {
        private readonly IWorkScheduleService workScheduleService;
        private readonly IWorkScheduleDayService workScheduleDayService;
        private readonly IMapper mapper;
        private readonly IValidator<WorkScheduleDayVM> validator;
        private const int WorkScheduleId = 1;

        public WorkScheduleController(IWorkScheduleService workScheduleService, IWorkScheduleDayService workScheduleDayService, IMapper mapper, IValidator<WorkScheduleDayVM> validator)
        {
            this.workScheduleService = workScheduleService;
            this.workScheduleDayService = workScheduleDayService;
            this.mapper = mapper;
            this.validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            List<WorkScheduleDay> WorkScheduleDays = await workScheduleService.GetScheduleDaysByIdAsync(WorkScheduleId);
            return View("Index", WorkScheduleDays);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var day = await workScheduleService.GetScheduleDayByIdAsync(WorkScheduleId, id);
            var dayVM = mapper.Map<WorkScheduleDayVM>(day);
            return View("Edit", dayVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(WorkScheduleDayVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var day = mapper.Map<WorkScheduleDay>(model);
                await workScheduleDayService.UpdateAsync(day);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View("Edit", model);
        }
    }
}
