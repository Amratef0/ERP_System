namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for payroll-related calculations (overtime, tax, deductions, allowances).
    /// </summary>
    public interface IPayrollCalculationService
    {
        /// <summary>
        /// Calculates overtime amount for an employee for a specific month.
        /// Overtime Rate = 1.5x base hourly rate.
        /// </summary>
        Task<decimal> CalculateOvertimeAmountAsync(int employeeId, int year, int month);

        /// <summary>
        /// Calculates tax amount (10% of gross salary).
        /// Gross = Base + Overtime + Bonus + Allowance
        /// </summary>
        decimal CalculateTaxAmount(decimal grossSalary);

        /// <summary>
        /// Calculates deductions for unpaid leave days.
        /// Deduction = (BaseSalary / WorkingDaysInMonth) * UnpaidLeaveDays
        /// </summary>
        Task<decimal> CalculateUnpaidLeaveDeductionAsync(int employeeId, int year, int month);

        /// <summary>
        /// Calculates bonus amount based on attendance and performance.
        /// Returns 0 if no bonus applicable (can be extended for performance bonuses).
        /// </summary>
        Task<decimal> CalculateBonusAmountAsync(int employeeId, int year, int month);

        /// <summary>
        /// Calculates allowance amount (currently returns 0, can be extended).
        /// </summary>
        Task<decimal> CalculateAllowanceAmountAsync(int employeeId, int year, int month);

        /// <summary>
        /// Gets the number of working days in a month (excluding weekends and holidays).
        /// </summary>
        Task<int> GetWorkingDaysInMonthAsync(int employeeId, int year, int month);

        /// <summary>
        /// Gets the standard monthly work hours for an employee.
        /// </summary>
        Task<decimal> GetStandardMonthlyHoursAsync(int employeeId, int year, int month);
    }
}
