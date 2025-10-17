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
        private readonly IAttendanceService _attendanceService;
        private readonly IWorkScheduleService _workScheduleService;
        private readonly IPublicHolidayService _publicHolidayService;
        private readonly ICountryService _countryService;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IJobTitleService _jobTitleService;
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
            _attendanceService = attendanceService;
            _workScheduleService = workScheduleService;
            _publicHolidayService = publicHolidayService;
            _countryService = countryService;
            _branchService = branchService;
            _departmentService = departmentService;
            _employeeTypeService = employeeTypeService;
            _jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AttendanceIndexVM model = new AttendanceIndexVM()
            {
                Countries = await _countryService.GetAllAsync(),
                Branches = await _branchService.GetAllAsync(),
                Departments = await _departmentService.GetAllAsync(),
                EmployeeTypes = await _employeeTypeService.GetAllAsync(),
                JobTitles = await _jobTitleService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttendance(DateOnly date, int countryId, string? name, int? branchId, int? departmentId, int? typeId, int? jobTitleId)
        {
            IEnumerable<EmployeeAttendanceRecordVM> attendanceRecords = await _attendanceService.GetAllByDateAsync(date, countryId, name, branchId, departmentId, typeId, jobTitleId);

            bool isDayOff = await _workScheduleService.CheckIfDayOffAsync(date, WorkScheduleId);
            bool isPublicHoliday = await _publicHolidayService.CheckIfPublicHolidayAsync(date, countryId);

            string infoMessage = null;

            if (isDayOff)
                infoMessage = "The selected date is a day off according to the work schedule.";
            else if (isPublicHoliday)
                infoMessage = "The selected date is a public holiday.";


            return Json(new { attendanceRecords, infoMessage });
        }
    }
}
