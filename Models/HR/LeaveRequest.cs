using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal TotalDays { get; set; }

        [MaxLength(255)]
        public string? Reason { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        [Required]
        [ForeignKey("StatusCode")]
        public int StatusCodeId { get; set; }
        public LeaveRequestStatusCode StatusCode { get; set; }

        [ForeignKey("ApprovedBy")]
        public int? ApprovedById { get; set; }
        public Employee? ApprovedBy { get; set; }
    }
}
