using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.HR
{
    public class WorkScheduleController : Controller
    {
        private readonly IWorkScheduleService _workScheduleService;
        private readonly IWorkScheduleDayService _workScheduleDayService;
        private readonly IMapper _mapper;
        private readonly IValidator<WorkScheduleDayVM> _validator;
        private const int WorkScheduleId = 1;

        public WorkScheduleController(IWorkScheduleService workScheduleService, IWorkScheduleDayService workScheduleDayService, IMapper mapper, IValidator<WorkScheduleDayVM> validator)
        {
            _workScheduleService = workScheduleService;
            _workScheduleDayService = workScheduleDayService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                ICollection<WorkScheduleDay> WorkScheduleDays = await _workScheduleService.GetScheduleDaysByIdAsync(WorkScheduleId);
                return View("Index", WorkScheduleDays);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the work schedule. Please try again.";
                return View("Index", new List<WorkScheduleDay>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            try
            {
                var day = await _workScheduleService.GetScheduleDayByIdAsync(WorkScheduleId, id);
                if (day == null)
                {
                    TempData["ErrorMessage"] = "Work Schedule Day not found!";
                    return NotFound();
                }
                var dayVM = _mapper.Map<WorkScheduleDayVM>(day);
                return View("Edit", dayVM);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the work schedule day.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAsync(WorkScheduleDayVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    var day = _mapper.Map<WorkScheduleDay>(model);
                    await _workScheduleDayService.UpdateAsync(day);
                    TempData["SuccessMessage"] = $"Work Schedule for {model.Day} has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _workScheduleService.GetScheduleDayByIdAsync(WorkScheduleId, model.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This work schedule day has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This work schedule was modified by another user.");
                        TempData["WarningMessage"] = "The work schedule was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("CHECK constraint"))
                    {
                        ModelState.AddModelError("", "Invalid time values. Please ensure start time is before end time.");
                        TempData["ErrorMessage"] = "Invalid time values. Start time must be before end time.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update work schedule due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the work schedule.";
                }
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View("Edit", model);
        }
    }
}
