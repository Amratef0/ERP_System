using ERP_System_Project.Models.ValidationAttributes;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class EmployeeLeaveBalance : ISoftDeletable
    {
        [Key]
        [Display(Name = "Leave Balance ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Total Entitled Days")]
        public decimal TotalEntitledDays { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Used Days")]
        public decimal UsedDays { get; set; }

        [DecimalPrecisionScale(5, 2)]
        [Display(Name = "Remaining Days")]
        public decimal RemainingDays { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [Required(ErrorMessage = "Employee is required.")]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required(ErrorMessage = "Leave type is required.")]
        [ForeignKey("LeaveType")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        public virtual LeaveType LeaveType { get; set; }
    }
}
