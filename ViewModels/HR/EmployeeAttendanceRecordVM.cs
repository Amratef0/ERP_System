using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class EmployeeAttendanceRecordVM
    {
        public int EmployeeId { get; set; }

        public int AttendanceRecordId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Branch { get; set; }

        public string Department { get; set; }

        public string Type { get; set; }

        public string JobTitle { get; set; }

        public string Status { get; set; }

        [Display(Name = "Attendance Date")]
        public DateOnly Date { get; set; }

        [Display(Name = "Actual Start Time")]
        public TimeOnly CheckInTime { get; set; }

        [Display(Name = "Actual End Time")]
        public TimeOnly? CheckOutTime { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Total Hours")]
        public decimal TotalHours { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Overtime Hours")]
        public decimal OverTimeHours { get; set; }

        [MaxLength(100, ErrorMessage = "Notes cannot exceed 100 characters.")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }
    }
}
