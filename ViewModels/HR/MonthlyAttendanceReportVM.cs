using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;

namespace ERP_System_Project.ViewModels.HR
{
    public class MonthlyAttendanceReportVM
    {
        // Filters Data
        public IEnumerable<Country> Countries { get; set; } = new List<Country>();
        public IEnumerable<Branch> Branches { get; set; } = new List<Branch>();
        public IEnumerable<Department> Departments { get; set; } = new List<Department>();
        public IEnumerable<EmployeeType> EmployeeTypes { get; set; } = new List<EmployeeType>();
        public IEnumerable<JobTitle> JobTitles { get; set; } = new List<JobTitle>();

        // Selected Filter Values
        public int? SelectedCountryId { get; set; }
        public int? SelectedBranchId { get; set; }
        public int? SelectedDepartmentId { get; set; }
        public int? SelectedEmployeeTypeId { get; set; }
        public int? SelectedJobTitleId { get; set; }
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        public int SelectedMonth { get; set; } = DateTime.Now.Month;

        // Report Data
        public IEnumerable<EmployeeMonthlyAttendanceSummary> EmployeeSummaries { get; set; } = new List<EmployeeMonthlyAttendanceSummary>();

        // Overall Statistics
        public int TotalEmployees { get; set; }
        public decimal AverageAttendanceRate { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }
    }

    public class EmployeeMonthlyAttendanceSummary
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;

        // Attendance Statistics
        public int TotalWorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public int LeaveDays { get; set; }

        // Hours Statistics
        public decimal TotalHoursWorked { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal AverageHoursPerDay { get; set; }

        // Calculated Fields
        public decimal AttendanceRate => TotalWorkingDays > 0 
            ? Math.Round((decimal)PresentDays / TotalWorkingDays * 100, 2) 
            : 0;

        public string AttendanceRateClass
        {
            get
            {
                if (AttendanceRate >= 95) return "bg-success";
                if (AttendanceRate >= 85) return "bg-info";
                if (AttendanceRate >= 75) return "bg-warning";
                return "bg-danger";
            }
        }
    }
}
