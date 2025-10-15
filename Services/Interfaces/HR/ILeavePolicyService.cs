using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for managing leave policies that determine employee leave entitlements.
    /// </summary>
    public interface ILeavePolicyService : IGenericService<LeavePolicy>
    {
        /// <summary>
        /// Calculates the entitled leave days for a specific employee and leave type.
        /// Uses hierarchy: JobTitle > EmployeeType > LeaveType Default
        /// </summary>
        /// <param name="employeeId">The employee ID</param>
        /// <param name="leaveTypeId">The leave type ID</param>
        /// <returns>The calculated entitled days</returns>
        Task<decimal> CalculateEntitledDaysAsync(int employeeId, int leaveTypeId);

        /// <summary>
        /// Gets all active policies for a specific leave type.
        /// </summary>
        /// <param name="leaveTypeId">The leave type ID</param>
        /// <returns>List of active policies</returns>
        Task<IEnumerable<LeavePolicy>> GetPoliciesByLeaveTypeAsync(int leaveTypeId);

        /// <summary>
        /// Gets the most applicable policy for an employee and leave type.
        /// </summary>
        /// <param name="employeeId">The employee ID</param>
        /// <param name="leaveTypeId">The leave type ID</param>
        /// <returns>The applicable leave policy or null</returns>
        Task<LeavePolicy?> GetApplicablePolicyAsync(int employeeId, int leaveTypeId);
    }
}
