using ERP_System_Project.Models.Interfaces;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    /// <summary>
    /// Defines leave entitlement policies based on JobTitle and EmployeeType.
    /// This table determines how many leave days an employee gets for each leave type.
    /// Priority: JobTitle (Primary) > EmployeeType (Secondary) > LeaveType Default
    /// </summary>
    public class LeavePolicy : ISoftDeletable
    {
        [Key]
        [Display(Name = "Policy ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Leave type is required.")]
        [ForeignKey("LeaveType")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        public virtual LeaveType LeaveType { get; set; }

        /// <summary>
        /// Primary factor: If specified, this policy applies to employees with this JobTitle.
        /// NULL means it applies to all job titles (used as fallback).
        /// </summary>
        [ForeignKey("JobTitle")]
        [Display(Name = "Job Title")]
        public int? JobTitleId { get; set; }
        public virtual JobTitle? JobTitle { get; set; }

        /// <summary>
        /// Secondary factor: If specified, this policy applies to employees with this EmployeeType.
        /// NULL means it applies to all employee types (used as fallback).
        /// </summary>
        [ForeignKey("EmployeeType")]
        [Display(Name = "Employee Type")]
        public int? EmployeeTypeId { get; set; }
        public virtual EmployeeType? EmployeeType { get; set; }

        [Required(ErrorMessage = "Entitled days is required.")]
        [DecimalPrecisionScale(5, 2)]
        [Range(0, 365, ErrorMessage = "Entitled days must be between 0 and 365.")]
        [Display(Name = "Entitled Days")]
        public decimal EntitledDays { get; set; }

        /// <summary>
        /// Higher priority value = higher precedence when multiple policies match.
        /// JobTitle-specific policies should have priority 10.
        /// EmployeeType-specific policies should have priority 5.
        /// Generic policies (no JobTitle/Type) should have priority 0.
        /// </summary>
        [Display(Name = "Priority")]
        public int Priority { get; set; } = 0;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
