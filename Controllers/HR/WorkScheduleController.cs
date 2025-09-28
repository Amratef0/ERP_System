using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class WorkScheduleController : Controller
    {
        private readonly IWorkScheduleService workScheduleService;
        private readonly IWorkScheduleDayService workScheduleDayService;
        private readonly IMapper mapper;
        private const int WorkScheduleId = 1;

        public WorkScheduleController(IWorkScheduleService workScheduleService, IWorkScheduleDayService workScheduleDayService, IMapper mapper)
        {
            this.workScheduleService = workScheduleService;
            this.workScheduleDayService = workScheduleDayService;
            this.mapper = mapper;
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
        public async Task<IActionResult> Edit(WorkScheduleDayVM model)
        {
            if (ModelState.IsValid)
            {
                var day = mapper.Map<WorkScheduleDay>(model);
                await workScheduleDayService.UpdateAsync(day);
                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }
    }
}
