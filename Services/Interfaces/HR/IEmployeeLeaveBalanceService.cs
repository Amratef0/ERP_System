using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for managing employee leave balances.
    /// Handles balance creation, updates, carry-forward, and integration with leave requests.
    /// </summary>
    public interface IEmployeeLeaveBalanceService : IGenericService<EmployeeLeaveBalance>
    {
        // ========== BALANCE RETRIEVAL ==========
        
        /// <summary>
        /// Gets the leave balance for a specific employee, leave type, and year.
        /// </summary>
        Task<EmployeeLeaveBalance?> GetBalanceAsync(int employeeId, int leaveTypeId, int year);

        /// <summary>
        /// Gets all balances for a specific employee in a given year.
        /// </summary>
        Task<IEnumerable<EmployeeLeaveBalance>> GetEmployeeBalancesAsync(int employeeId, int year);

        /// <summary>
        /// Gets all balances with filters for HR manager view.
        /// </summary>
        Task<IEnumerable<EmployeeLeaveBalanceVM>> GetAllBalancesAsync(
            int? year = null,
            int? leaveTypeId = null,
            string? employeeName = null,
            int? branchId = null,
            int? departmentId = null,
            int? employeeTypeId = null,
            int? jobTitleId = null);

        // ========== BALANCE CREATION ==========

        /// <summary>
        /// Creates leave balances for a new employee for the current year.
        /// Called automatically when a new employee is hired.
        /// </summary>
        Task<bool> GenerateBalancesForNewEmployeeAsync(int employeeId);

        /// <summary>
        /// Generates balances for all active employees for a specific year.
        /// Used for new year setup or bulk generation.
        /// </summary>
        Task<bool> GenerateBalancesForAllEmployeesAsync(int year, int? leaveTypeId = null);

        /// <summary>
        /// Manually creates a balance record (for special cases or corrections).
        /// </summary>
        Task<bool> CreateBalanceAsync(EmployeeLeaveBalanceVM model);

        // ========== BALANCE UPDATES ==========

        /// <summary>
        /// Updates the TotalEntitledDays for a balance (manual adjustment by HR).
        /// </summary>
        Task<bool> UpdateBalanceEntitlementAsync(int balanceId, decimal newEntitledDays);

        /// <summary>
        /// Updates UsedDays when a leave request is approved.
        /// Called automatically from LeaveRequest approval process.
        /// </summary>
        Task<bool> UpdateUsedDaysAsync(int employeeId, int leaveTypeId, int year, decimal daysToAdd);

        /// <summary>
        /// Reverses UsedDays when a leave request is rejected or deleted.
        /// </summary>
        Task<bool> ReverseUsedDaysAsync(int employeeId, int leaveTypeId, int year, decimal daysToSubtract);

        // ========== CARRY FORWARD ==========

        /// <summary>
        /// Carries forward unused leave days from one year to the next.
        /// Runs automatically at year-end or can be triggered manually.
        /// </summary>
        Task<bool> CarryForwardUnusedDaysAsync(int fromYear, int toYear);

        /// <summary>
        /// Carries forward unused days for a specific employee.
        /// </summary>
        Task<bool> CarryForwardEmployeeBalancesAsync(int employeeId, int fromYear, int toYear);

        // ========== VALIDATION ==========

        /// <summary>
        /// Validates if an employee has sufficient balance for a leave request.
        /// </summary>
        Task<(bool IsValid, string? ErrorMessage)> ValidateLeaveRequestAsync(
            int employeeId, 
            int leaveTypeId, 
            int year, 
            decimal requestedDays);

        /// <summary>
        /// Checks if a balance already exists for employee/leave type/year.
        /// </summary>
        Task<bool> BalanceExistsAsync(int employeeId, int leaveTypeId, int year);

        /// <summary>
        /// Gets a balance by ID with all navigation properties loaded.
        /// </summary>
        Task<EmployeeLeaveBalance?> GetByIdWithDetailsAsync(int id);
    }
}
