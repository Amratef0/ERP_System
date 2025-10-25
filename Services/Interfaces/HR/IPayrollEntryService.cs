using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for managing individual payroll entries (per employee per month).
    /// </summary>
    public interface IPayrollEntryService : IGenericService<PayrollEntry>
    {
        /// <summary>
        /// Creates a payroll entry for an employee with automatic calculations.
        /// </summary>
        Task<(bool Success, PayrollEntry? Entry, string? ErrorMessage)> CreatePayrollEntryAsync(
            int payrollRunId,
            int employeeId,
            int year,
            int month);

        /// <summary>
        /// Updates an existing payroll entry (only if parent PayrollRun is not locked).
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> UpdatePayrollEntryAsync(PayrollEntryVM model);

        /// <summary>
        /// Gets all entries for a specific payroll run.
        /// </summary>
        Task<IEnumerable<PayrollEntryVM>> GetEntriesByPayrollRunAsync(int payrollRunId);

        /// <summary>
        /// Gets payroll entry with full details.
        /// </summary>
        Task<PayrollEntryVM?> GetPayrollEntryDetailsAsync(int entryId);

        /// <summary>
        /// Deletes an entry (only if parent PayrollRun is not locked).
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> DeletePayrollEntryAsync(int entryId);

        /// <summary>
        /// Calculates net amount for a payroll entry.
        /// Net = Base + Overtime + Bonus + Allowance - Deduction - Tax
        /// </summary>
        decimal CalculateNetAmount(decimal baseSalary, decimal overtime, decimal bonus, 
            decimal allowance, decimal deduction, decimal tax);
    }
}
