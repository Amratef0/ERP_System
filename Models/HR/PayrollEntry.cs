using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ValidationAttributes;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PayrollEntry : ISoftDeletable
    {
        [Key]
        [Display(Name = "Payroll Entry ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Base Salary Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Base Salary Amount")]
        public decimal BaseSalaryAmount { get; set; }

        [Required(ErrorMessage = "Overtime Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Overtime Amount")]
        public decimal OvertimeAmount { get; set; }

        [Required(ErrorMessage = "Bonus Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Bonus Amount")]
        public decimal BonusAmount { get; set; }

        [Required(ErrorMessage = "Allowance Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Allowance Amount")]
        public decimal AllowanceAmount { get; set; }

        [Required(ErrorMessage = "Deduction Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Deduction Amount")]
        public decimal DeductionAmount { get; set; }

        [Required(ErrorMessage = "Tax Amount is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Payment Date")]
        public DateOnly? PaymentDate { get; set; }

        [MaxLength(100, ErrorMessage = "Bank Account Number cannot exceed 100 characters.")]
        [Display(Name = "Bank Account Number")]
        public string? BankAccountNumber { get; set; }

        [MaxLength(255, ErrorMessage = "Notes cannot exceed 255 characters.")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation properties

        [Required(ErrorMessage = "Payroll Run is required.")]
        [ForeignKey("PayrollRun")]
        [Display(Name = "Payroll Run")]
        public int PayrollRunId { get; set; }
        public virtual PayrollRun PayrollRun { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [ForeignKey("Currency")]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
