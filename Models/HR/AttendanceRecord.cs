using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        public DateTime? ScheduledStartTime { get; set; }

        public DateTime? ScheduledEndTime { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal? ScheduledHours { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal? TotalHours { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal? OverTimeHours { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("StatusCode")]
        public int StatusCodeId { get; set; }
        public AttendanceStatusCode StatusCode { get; set; }
    }
}
