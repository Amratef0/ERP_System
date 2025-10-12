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
                Types = await employeeTypeService.GetAllAsync(),
                JobTitles = await jobTitleService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendance(DateOnly date, int countryId, int? branchId, int? departmentId, int? typeId, int? jobTitleId)
        {
            IEnumerable<EmployeeAttendanceRecordVM> attendanceRecords = await attendanceService.GetAllByDateAsync(date, countryId, branchId, departmentId, typeId, jobTitleId);
            bool isPublicHoliday = await publicHolidayService.CheckIfPublicHolidayAsync(date, countryId);
            bool isDayOff = await workScheduleService.CheckIfDayOffAsync(date, WorkScheduleId);
            return Json(new
            {
                attendanceRecords,
                isPublicHoliday,
                isDayOff
            });
        }
    }
}
