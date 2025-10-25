using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ValidationAttributes;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PayrollRun : ISoftDeletable
    {
        [Key]
        [Display(Name = "Payroll Run ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Period Start date is required.")]
        [Display(Name = "Period Start Date")]
        public DateOnly PeriodStart { get; set; }

        [Required(ErrorMessage = "Period End date is required.")]
        [Display(Name = "Period End Date")]
        public DateOnly PeriodEnd { get; set; }

        [Required(ErrorMessage = "Payroll Run Name is required.")]
        [MaxLength(150, ErrorMessage = "Payroll Run Name cannot exceed 150 characters.")]
        [Display(Name = "Payroll Run Name")]
        public string Name { get; set; }

        [Display(Name = "Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [Display(Name = "Is Locked")]
        public bool IsLocked { get; set; }

        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Total Amount")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation properties
        [Display(Name = "Payroll Entries")]
        public virtual ICollection<PayrollEntry> PayrollEntries { get; set; } = new List<PayrollEntry>();
    }
}
