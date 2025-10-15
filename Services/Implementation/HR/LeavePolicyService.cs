using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    /// <summary>
    /// Service for managing leave policies.
    /// Handles calculation of entitled leave days based on employee properties.
    /// </summary>
    public class LeavePolicyService : GenericService<LeavePolicy>, ILeavePolicyService
    {
        public LeavePolicyService(IUnitOfWork uow) : base(uow)
        {
        }

        /// <summary>
        /// Calculates entitled days using policy hierarchy:
        /// 1. JobTitle-specific policy (Priority 10)
        /// 2. EmployeeType-specific policy (Priority 5)
        /// 3. Generic policy (Priority 0)
        /// 4. LeaveType.MaxDaysPerYear as final fallback
        /// </summary>
        public async Task<decimal> CalculateEntitledDaysAsync(int employeeId, int leaveTypeId)
        {
            // Get employee details
            var employee = await _uow.Employees
                .GetAllAsIQueryable()
                .Include(e => e.JobTitle)
                .Include(e => e.Type)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new ArgumentException($"Employee with ID {employeeId} not found.");

            // Get all active policies for this leave type, ordered by priority (descending)
            var policies = await _repository
                .GetAllAsIQueryable()
                .Where(lp => lp.LeaveTypeId == leaveTypeId && lp.IsActive)
                .OrderByDescending(lp => lp.Priority)
                .ToListAsync();

            // 1. Try to find JobTitle-specific policy
            var jobTitlePolicy = policies.FirstOrDefault(p => 
                p.JobTitleId == employee.JobTitleId && p.EmployeeTypeId == null);
            
            if (jobTitlePolicy != null)
                return jobTitlePolicy.EntitledDays;

            // 2. Try to find EmployeeType-specific policy
            var employeeTypePolicy = policies.FirstOrDefault(p => 
                p.EmployeeTypeId == employee.TypeId && p.JobTitleId == null);
            
            if (employeeTypePolicy != null)
                return employeeTypePolicy.EntitledDays;

            // 3. Try to find generic policy (no JobTitle or EmployeeType specified)
            var genericPolicy = policies.FirstOrDefault(p => 
                p.JobTitleId == null && p.EmployeeTypeId == null);
            
            if (genericPolicy != null)
                return genericPolicy.EntitledDays;

            // 4. Final fallback: Use LeaveType.MaxDaysPerYear
            var leaveType = await _uow.LeaveTypes.GetByIdAsync(leaveTypeId);
            if (leaveType?.MaxDaysPerYear != null)
                return leaveType.MaxDaysPerYear.Value;

            // If no policy or default found, return 0
            return 0;
        }

        public async Task<IEnumerable<LeavePolicy>> GetPoliciesByLeaveTypeAsync(int leaveTypeId)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(lp => lp.LeaveType)
                .Include(lp => lp.JobTitle)
                .Include(lp => lp.EmployeeType)
                .Where(lp => lp.LeaveTypeId == leaveTypeId && lp.IsActive)
                .OrderByDescending(lp => lp.Priority)
                .ToListAsync();
        }

        public async Task<LeavePolicy?> GetApplicablePolicyAsync(int employeeId, int leaveTypeId)
        {
            var employee = await _uow.Employees
                .GetAllAsIQueryable()
                .Include(e => e.JobTitle)
                .Include(e => e.Type)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return null;

            var policies = await _repository
                .GetAllAsIQueryable()
                .Where(lp => lp.LeaveTypeId == leaveTypeId && lp.IsActive)
                .OrderByDescending(lp => lp.Priority)
                .ToListAsync();

            // Check in priority order
            return policies.FirstOrDefault(p => p.JobTitleId == employee.JobTitleId && p.EmployeeTypeId == null)
                ?? policies.FirstOrDefault(p => p.EmployeeTypeId == employee.TypeId && p.JobTitleId == null)
                ?? policies.FirstOrDefault(p => p.JobTitleId == null && p.EmployeeTypeId == null);
        }
    }
}
