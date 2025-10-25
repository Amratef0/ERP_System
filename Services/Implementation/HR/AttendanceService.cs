using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class AttendanceService : GenericService<AttendanceRecord>, IAttendanceService
    {
        private readonly IMapper mapper;

        public AttendanceService(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeAttendanceRecordVM>> GetAllByDateAsync(
            DateOnly date,
            int countryId,
            string? name,
            int? branchId,
            int? departmentId,
            int? typeId,
            int? jobTitleId)
        {
            var query = _repository.GetAllAsIQueryable();

            query = query.Include(ar => ar.Employee)
                            .ThenInclude(e => e.Branch)
                                .ThenInclude(b => b.Address)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Department)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Type)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.JobTitle)
                         .Include(ar => ar.StatusCode)
                         .Where(ar => ar.Date == date && ar.Employee.Branch.Address.CountryId == countryId);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(ar => (ar.Employee.FirstName + " " + ar.Employee.LastName).ToUpper().StartsWith(name.ToUpper()));
            }

            if (branchId.HasValue && branchId > 0)
            {
                query = query.Where(ar => ar.Employee.BranchId == branchId.Value);
            }

            if (departmentId.HasValue && departmentId > 0)
            {
                query = query.Where(ar => ar.Employee.DepartmentId == departmentId.Value);
            }

            if (typeId.HasValue && typeId > 0)
            {
                query = query.Where(ar => ar.Employee.TypeId == typeId.Value);
            }

            if (jobTitleId.HasValue && jobTitleId > 0)
            {
                query = query.Where(ar => ar.Employee.JobTitleId == jobTitleId.Value);
            }

            IEnumerable<AttendanceRecord> attendanceRecords = await query.ToListAsync();

            IEnumerable<EmployeeAttendanceRecordVM> attendanceRecordVMs = mapper.Map<IEnumerable<EmployeeAttendanceRecordVM>>(attendanceRecords);

            return attendanceRecordVMs;
        }

        public async Task<IEnumerable<EmployeeMonthlyAttendanceSummary>> GetMonthlyAttendanceSummaryAsync(
            int year,
            int month,
            int? countryId = null,
            int? branchId = null,
            int? departmentId = null,
            int? employeeTypeId = null,
            int? jobTitleId = null)
        {
            // Get start and end date of the month
            var startDate = new DateOnly(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            // Build query
            var query = _repository.GetAllAsIQueryable()
                .Include(ar => ar.Employee)
                    .ThenInclude(e => e.Branch)
                        .ThenInclude(b => b.Address)
                .Include(ar => ar.Employee)
                    .ThenInclude(e => e.Department)
                .Include(ar => ar.Employee)
                    .ThenInclude(e => e.Type)
                .Include(ar => ar.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(ar => ar.StatusCode)
                .Where(ar => ar.Date >= startDate && ar.Date <= endDate);

            // Apply filters
            if (countryId.HasValue && countryId > 0)
            {
                query = query.Where(ar => ar.Employee.Branch.Address.CountryId == countryId.Value);
            }

            if (branchId.HasValue && branchId > 0)
            {
                query = query.Where(ar => ar.Employee.BranchId == branchId.Value);
            }

            if (departmentId.HasValue && departmentId > 0)
            {
                query = query.Where(ar => ar.Employee.DepartmentId == departmentId.Value);
            }

            if (employeeTypeId.HasValue && employeeTypeId > 0)
            {
                query = query.Where(ar => ar.Employee.TypeId == employeeTypeId.Value);
            }

            if (jobTitleId.HasValue && jobTitleId > 0)
            {
                query = query.Where(ar => ar.Employee.JobTitleId == jobTitleId.Value);
            }

            var attendanceRecords = await query.ToListAsync();

            // Calculate working days in month (excluding weekends - simple calculation)
            var totalDaysInMonth = DateTime.DaysInMonth(year, month);
            var workingDays = 0;
            for (int day = 1; day <= totalDaysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            // Group by employee and calculate summaries
            var summaries = attendanceRecords
                .GroupBy(ar => ar.Employee)
                .Select(g => new EmployeeMonthlyAttendanceSummary
                {
                    EmployeeId = g.Key.Id,
                    FullName = $"{g.Key.FirstName} {g.Key.LastName}",
                    Branch = g.Key.Branch?.Name ?? "N/A",
                    Department = g.Key.Department?.Name ?? "N/A",
                    EmployeeType = g.Key.Type?.Name ?? "N/A",
                    JobTitle = g.Key.JobTitle?.Name ?? "N/A",
                    TotalWorkingDays = workingDays,
                    PresentDays = g.Count(ar => ar.StatusCode != null && ar.StatusCode.Name.ToLower() == "present"),
                    AbsentDays = g.Count(ar => ar.StatusCode != null && ar.StatusCode.Name.ToLower() == "absent"),
                    LateDays = g.Count(ar => ar.StatusCode != null && ar.StatusCode.Name.ToLower() == "late"),
                    LeaveDays = g.Count(ar => ar.StatusCode != null && ar.StatusCode.Name.ToLower().Contains("leave")),
                    TotalHoursWorked = g.Sum(ar => ar.TotalHours),
                    OvertimeHours = g.Sum(ar => ar.OverTimeHours),
                    AverageHoursPerDay = g.Any() ? Math.Round(g.Average(ar => ar.TotalHours), 2) : 0
                })
                .OrderBy(s => s.FullName)
                .ToList();

            return summaries;
        }
    }
}
