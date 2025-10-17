using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    /// <summary>
    /// ViewModel for Employee Leave Balance management by HR.
    /// </summary>
    public class EmployeeLeaveBalanceVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Leave type is required.")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Leave Type")]
        public string? LeaveTypeName { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100.")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Total entitled days is required.")]
        [Range(0, 365, ErrorMessage = "Entitled days must be between 0 and 365.")]
        [Display(Name = "Total Entitled Days")]
        public decimal TotalEntitledDays { get; set; }

        [Display(Name = "Used Days")]
        public decimal UsedDays { get; set; }

        [Display(Name = "Remaining Days")]
        public decimal RemainingDays { get; set; }

        // For display purposes
        [Display(Name = "Branch")]
        public string? BranchName { get; set; }

        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }

        [Display(Name = "Job Title")]
        public string? JobTitleName { get; set; }

        [Display(Name = "Employee Type")]
        public string? EmployeeTypeName { get; set; }

        // For filtering in Index view
        public IEnumerable<Employee>? Employees { get; set; }
        public IEnumerable<LeaveType>? LeaveTypes { get; set; }
        public IEnumerable<Branch>? Branches { get; set; }
        public IEnumerable<Department>? Departments { get; set; }
        public IEnumerable<JobTitle>? JobTitles { get; set; }
        public IEnumerable<EmployeeType>? EmployeeTypes { get; set; }
    }
}
