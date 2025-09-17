using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PayrollEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal BaseSalaryAmount { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal OvertimeAmount { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal BonusAmount { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal AllowanceAmount { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal DeductionAmount { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal TaxAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal NetAmount { get; set; }

        public DateOnly? PaymentDate { get; set; }

        [MaxLength(100)]
        public string? BankAccountNumber { get; set; }

        [MaxLength(255)]
        public string? Notes { get; set; }

        // Navigation properties

        [Required]
        [ForeignKey("PayrollRun")]
        public int PayrollRunId { get; set; }
        public PayrollRun PayrollRun { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
