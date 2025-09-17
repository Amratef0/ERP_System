using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class EmployeeLeaveBalance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal TotalEntitledDays { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal UsedDays { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal RemainingDays { get; set; }

        // Navigation Properties

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
    }
}
