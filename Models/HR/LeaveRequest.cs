using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class LeaveRequest : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Range(0.5, 365, ErrorMessage = "Total days must be between 0.5 and 365.")]
        [Display(Name = "Total Days")]
        public decimal TotalDays { get; set; }

        [MaxLength(255, ErrorMessage = "Reason cannot exceed 255 characters.")]
        [Display(Name = "Reason for Leave")]
        public string? Reason { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties

        [Required(ErrorMessage = "Employee is required.")]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Leave Type is required.")]
        [ForeignKey("LeaveType")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [ForeignKey("StatusCode")]
        [Display(Name = "Status")]
        public int StatusCodeId { get; set; }
        public LeaveRequestStatusCode StatusCode { get; set; }

        [ForeignKey("ApprovedBy")]
        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }
        public Employee? ApprovedBy { get; set; }

        // Custom validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return new ValidationResult(
                    "End Date cannot be earlier than Start Date.",
                    new[] { nameof(EndDate), nameof(StartDate) }
                );
            }
        }
    }
}
