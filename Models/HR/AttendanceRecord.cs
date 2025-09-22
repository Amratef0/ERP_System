using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class AttendanceRecord
    {
        [Key]
        [Display(Name = "Record ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Attendance Date")]
        public DateOnly Date { get; set; }

        [Display(Name = "Scheduled Start Time")]
        public DateTime? ScheduledStartTime { get; set; }

        [Display(Name = "Scheduled End Time")]
        public DateTime? ScheduledEndTime { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Scheduled Hours")]
        public decimal? ScheduledHours { get; set; }

        [Display(Name = "Actual Start Time")]
        public DateTime? ActualStartTime { get; set; }

        [Display(Name = "Actual End Time")]
        public DateTime? ActualEndTime { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Total Hours")]
        public decimal? TotalHours { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Overtime Hours")]
        public decimal? OverTimeHours { get; set; }

        [MaxLength(100, ErrorMessage = "Notes cannot exceed 100 characters.")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        [Required(ErrorMessage = "Employee is required.")]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Status code is required.")]
        [ForeignKey("StatusCode")]
        [Display(Name = "Attendance Status")]
        public int StatusCodeId { get; set; }

        [Display(Name = "Attendance Status")]
        public AttendanceStatusCode StatusCode { get; set; }
    }
}
