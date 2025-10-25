using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for managing payroll runs (monthly payroll cycles).
    /// </summary>
    public interface IPayrollRunService : IGenericService<PayrollRun>
    {
        /// <summary>
        /// Generates a draft payroll run for a specific month and year.
        /// Creates PayrollEntry for each active employee.
        /// </summary>
        Task<(bool Success, int? PayrollRunId, string? ErrorMessage)> GeneratePayrollRunAsync(int year, int month);

        /// <summary>
        /// Re-generates/recalculates a draft payroll run (deletes old entries and creates new ones).
        /// Only works if IsLocked = false.
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> RegeneratePayrollRunAsync(int payrollRunId);

        /// <summary>
        /// Locks a payroll run after HR approval (prevents further editing).
        /// Sends email notifications to all employees.
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> LockPayrollRunAsync(int payrollRunId);

        /// <summary>
        /// Unlocks a payroll run (for corrections or adjustments).
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> UnlockPayrollRunAsync(int payrollRunId);

        /// <summary>
        /// Gets all payroll runs with filtering options.
        /// </summary>
        Task<IEnumerable<PayrollRunVM>> GetAllPayrollRunsAsync(int? year = null, int? month = null, bool? isLocked = null);

        /// <summary>
        /// Gets a payroll run with all its entries and employee details.
        /// </summary>
        Task<PayrollRunDetailsVM?> GetPayrollRunDetailsAsync(int payrollRunId);

        /// <summary>
        /// Deletes a payroll run (only if not locked).
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> DeletePayrollRunAsync(int payrollRunId);

        /// <summary>
        /// Checks if a payroll run already exists for a specific period.
        /// </summary>
        Task<bool> PayrollRunExistsAsync(int year, int month);

        /// <summary>
        /// Gets payroll run by year and month.
        /// </summary>
        Task<PayrollRun?> GetPayrollRunByPeriodAsync(int year, int month);
    }
}
