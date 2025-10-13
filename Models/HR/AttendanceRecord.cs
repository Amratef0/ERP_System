using ERP_System_Project.Models.ValidationAttributes;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class AttendanceRecord : ISoftDeletable
    {
        [Key]
        [Display(Name = "Record ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
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

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [Required(ErrorMessage = "Employee is required.")]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee")]
        public virtual Employee Employee { get; set; }

        [Required(ErrorMessage = "Status code is required.")]
        [ForeignKey("StatusCode")]
        [Display(Name = "Attendance Status")]
        public int StatusCodeId { get; set; }

        [Display(Name = "Attendance Status")]
        public virtual AttendanceStatusCode StatusCode { get; set; }
    }
}
