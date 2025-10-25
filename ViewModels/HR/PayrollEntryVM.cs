using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class PayrollEntryVM
    {
        public int Id { get; set; }

        [Display(Name = "Payroll Run")]
        public int PayrollRunId { get; set; }

        [Display(Name = "Payroll Run Name")]
        public string? PayrollRunName { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Display(Name = "Employee Email")]
        public string? EmployeeEmail { get; set; }

        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }

        [Display(Name = "Job Title")]
        public string? JobTitleName { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Currency Code")]
        public string? CurrencyCode { get; set; }

        [Required(ErrorMessage = "Base salary amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Base salary must be positive.")]
        [Display(Name = "Base Salary")]
        public decimal BaseSalaryAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Overtime amount must be positive.")]
        [Display(Name = "Overtime")]
        public decimal OvertimeAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Bonus amount must be positive.")]
        [Display(Name = "Bonus")]
        public decimal BonusAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Allowance amount must be positive.")]
        [Display(Name = "Allowance")]
        public decimal AllowanceAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deduction amount must be positive.")]
        [Display(Name = "Deductions")]
        public decimal DeductionAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be positive.")]
        [Display(Name = "Tax (10%)")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Payment Date")]
        public DateOnly? PaymentDate { get; set; }

        [Display(Name = "Bank Account")]
        public string? BankAccountNumber { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        // Computed fields for display
        [Display(Name = "Gross Salary")]
        public decimal GrossSalary => BaseSalaryAmount + OvertimeAmount + BonusAmount + AllowanceAmount;

        [Display(Name = "Total Deductions")]
        public decimal TotalDeductions => DeductionAmount + TaxAmount;

        public bool IsLocked { get; set; }
    }
}
