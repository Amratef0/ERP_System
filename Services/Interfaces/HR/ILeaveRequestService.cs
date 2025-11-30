using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    /// <summary>
    /// Service for managing employee leave requests.
    /// </summary>
    public interface ILeaveRequestService : IGenericService<LeaveRequest>
    {
        /// <summary>
        /// Gets all leave requests for a specific employee.
        /// </summary>
        Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveRequestsAsync(int employeeId);

        /// <summary>
        /// Gets leave requests by status for an employee.
        /// </summary>
        Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveRequestsByStatusAsync(int employeeId, LeaveRequestStatus status);

        /// <summary>
        /// Gets pending leave requests for approval (for managers).
        /// </summary>
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync(int? departmentId = null);

        /// <summary>
        /// Gets approved leave requests (for team calendar).
        /// </summary>
        Task<IEnumerable<LeaveRequest>> GetApprovedLeaveRequestsAsync(int? departmentId = null);

        /// <summary>
        /// Creates a leave request and validates against balance.
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> CreateLeaveRequestAsync(LeaveRequest leaveRequest);

        /// <summary>
        /// Approves a leave request and updates leave balance.
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> ApproveLeaveRequestAsync(int leaveRequestId, int approvedById);

        /// <summary>
        /// Rejects a leave request.
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> RejectLeaveRequestAsync(int leaveRequestId, int approvedById);

        /// <summary>
        /// Cancels a leave request (by employee before approval).
        /// </summary>
        Task<(bool Success, string? ErrorMessage)> CancelLeaveRequestAsync(int leaveRequestId, int employeeId);

        /// <summary>
        /// Gets leave request with all details loaded.
        /// </summary>
        Task<LeaveRequest?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Calculates business days between two dates excluding weekends and holidays.
        /// </summary>
        Task<decimal> CalculateLeaveDaysAsync(DateOnly startDate, DateOnly endDate, int employeeId);
    }
}
