using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.HR
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService attendanceService;
        private readonly IWorkScheduleService workScheduleService;
        private readonly IPublicHolidayService publicHolidayService;
        private readonly ICountryService countryService;
        private readonly IBranchService branchService;
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeTypeService employeeTypeService;
        private readonly IJobTitleService jobTitleService;
        private const int WorkScheduleId = 1;

        public AttendanceController(
            IAttendanceService attendanceService,
            IWorkScheduleService workScheduleService,
            IPublicHolidayService publicHolidayService,
            ICountryService countryService,
            IBranchService branchService,
            IDepartmentService departmentService,
            IEmployeeTypeService employeeTypeService,
            IJobTitleService jobTitleService)
        {
            this.attendanceService = attendanceService;
            this.workScheduleService = workScheduleService;
            this.publicHolidayService = publicHolidayService;
            this.countryService = countryService;
            this.branchService = branchService;
            this.departmentService = departmentService;
            this.employeeTypeService = employeeTypeService;
            this.jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AttendanceIndexVM model = new AttendanceIndexVM()
            {
                Countries = await countryService.GetAllAsync(),
                Branches = await branchService.GetAllAsync(),
                Departments = await departmentService.GetAllAsync(),
                EmployeeTypes = await employeeTypeService.GetAllAsync(),
                JobTitles = await jobTitleService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttendance(DateOnly date, int countryId, string? name, int? branchId, int? departmentId, int? typeId, int? jobTitleId)
        {
            IEnumerable<EmployeeAttendanceRecordVM> attendanceRecords = await attendanceService.GetAllByDateAsync(date, countryId, name, branchId, departmentId, typeId, jobTitleId);

            bool isDayOff = await workScheduleService.CheckIfDayOffAsync(date, WorkScheduleId);
            bool isPublicHoliday = await publicHolidayService.CheckIfPublicHolidayAsync(date, countryId);

            string infoMessage = null;

            if (isDayOff)
                infoMessage = "The selected date is a day off according to the work schedule.";
            else if (isPublicHoliday)
                infoMessage = "The selected date is a public holiday.";


            return Json(new { attendanceRecords, infoMessage });
        }
    }
}
