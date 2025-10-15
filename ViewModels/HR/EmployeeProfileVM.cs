using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    /// <summary>
    /// ViewModel for employee profile page.
    /// </summary>
    public class EmployeeProfileVM
    {
        // Employee Information
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ImageURL { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly HireDate { get; set; }
        
        // Job Information
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobTitleName { get; set; }
        public string? EmployeeTypeName { get; set; }
        
        // Leave Balance Summary
        public IEnumerable<EmployeeLeaveBalance> LeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();
        
        // Recent Leave Requests
        public IEnumerable<LeaveRequest> RecentLeaveRequests { get; set; } = new List<LeaveRequest>();
        
        // Statistics
        public int PendingRequestsCount { get; set; }
        public int ApprovedRequestsThisYear { get; set; }
    }

    /// <summary>
    /// ViewModel for creating/editing leave requests.
    /// </summary>
    public class LeaveRequestVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Leave type is required.")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Total Days")]
        public decimal TotalDays { get; set; }

        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        [Display(Name = "Reason for Leave")]
        public string? Reason { get; set; }

        [Display(Name = "Status")]
        public LeaveRequestStatus Status { get; set; }

        // For display
        public string? LeaveTypeName { get; set; }
        public decimal? AvailableBalance { get; set; }
        
        // Dropdown data
        public IEnumerable<LeaveType>? LeaveTypes { get; set; }
    }
}
