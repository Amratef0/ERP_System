using AutoMapper;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    /// <summary>
    /// Service for managing employee leave balances.
    /// Handles creation, updates, carry-forward, and integration with leave requests.
    /// </summary>
    public class EmployeeLeaveBalanceService : GenericService<EmployeeLeaveBalance>, IEmployeeLeaveBalanceService
    {
        private readonly ILeavePolicyService _leavePolicyService;
        private readonly IMapper _mapper;

        public EmployeeLeaveBalanceService(
            IUnitOfWork uow, 
            ILeavePolicyService leavePolicyService,
            IMapper mapper) : base(uow)
        {
            _leavePolicyService = leavePolicyService;
            _mapper = mapper;
        }

        // ========== BALANCE RETRIEVAL ==========

        public async Task<EmployeeLeaveBalance?> GetBalanceAsync(int employeeId, int leaveTypeId, int year)
        {
            return await _repository
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(b => 
                    b.EmployeeId == employeeId && 
                    b.LeaveTypeId == leaveTypeId && 
                    b.Year == year);
        }

        public async Task<IEnumerable<EmployeeLeaveBalance>> GetEmployeeBalancesAsync(int employeeId, int year)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(b => b.LeaveType)
                .Where(b => b.EmployeeId == employeeId && b.Year == year)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeLeaveBalanceVM>> GetAllBalancesAsync(
            int? year = null,
            int? leaveTypeId = null,
            string? employeeName = null,
            int? branchId = null,
            int? departmentId = null,
            int? employeeTypeId = null,
            int? jobTitleId = null)
        {
            var query = _repository.GetAllAsIQueryable()
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Branch)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Department)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Type)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(b => b.LeaveType)
                .AsQueryable();

            // Apply filters
            if (year.HasValue)
                query = query.Where(b => b.Year == year.Value);

            if (leaveTypeId.HasValue)
                query = query.Where(b => b.LeaveTypeId == leaveTypeId.Value);

            if (!string.IsNullOrWhiteSpace(employeeName))
                query = query.Where(b => (b.Employee.FirstName + " " + b.Employee.LastName).Contains(employeeName));

            if (branchId.HasValue && branchId > 0)
                query = query.Where(b => b.Employee.BranchId == branchId.Value);

            if (departmentId.HasValue && departmentId > 0)
                query = query.Where(b => b.Employee.DepartmentId == departmentId.Value);

            if (employeeTypeId.HasValue && employeeTypeId > 0)
                query = query.Where(b => b.Employee.TypeId == employeeTypeId.Value);

            if (jobTitleId.HasValue && jobTitleId > 0)
                query = query.Where(b => b.Employee.JobTitleId == jobTitleId.Value);

            var balances = await query.ToListAsync();
            return _mapper.Map<IEnumerable<EmployeeLeaveBalanceVM>>(balances);
        }

        // ========== BALANCE CREATION ==========

        /// <summary>
        /// Creates balances for a new employee for the current year.
        /// Called when employee is hired.
        /// </summary>
        public async Task<bool> GenerateBalancesForNewEmployeeAsync(int employeeId)
        {
            int currentYear = DateTime.Now.Year;
            
            // Get all active leave types
            var leaveTypes = await _uow.LeaveTypes
                .GetAllAsIQueryable()
                .Where(lt => lt.IsActive)
                .ToListAsync();

            foreach (var leaveType in leaveTypes)
            {
                // Check if balance already exists
                if (await BalanceExistsAsync(employeeId, leaveType.Id, currentYear))
                    continue;

                // Calculate entitled days from policy
                var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(employeeId, leaveType.Id);

                // Create balance
                var balance = new EmployeeLeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    Year = currentYear,
                    TotalEntitledDays = entitledDays,
                    UsedDays = 0
                    // RemainingDays is computed automatically
                };

                await _repository.AddAsync(balance);
            }

            return await _uow.CompleteAsync() > 0;
        }

        /// <summary>
        /// Generates balances for all active employees for a specific year.
        /// Optionally can generate for a specific leave type only.
        /// </summary>
        public async Task<bool> GenerateBalancesForAllEmployeesAsync(int year, int? leaveTypeId = null)
        {
            // Get all active employees
            var employees = await _uow.Employees
                .GetAllAsIQueryable()
                .Where(e => e.IsActive)
                .ToListAsync();

            // Get leave types to generate balances for
            var leaveTypesQuery = _uow.LeaveTypes.GetAllAsIQueryable().Where(lt => lt.IsActive);
            
            if (leaveTypeId.HasValue)
                leaveTypesQuery = leaveTypesQuery.Where(lt => lt.Id == leaveTypeId.Value);

            var leaveTypes = await leaveTypesQuery.ToListAsync();

            int createdCount = 0;

            foreach (var employee in employees)
            {
                foreach (var leaveType in leaveTypes)
                {
                    // Skip if balance already exists
                    if (await BalanceExistsAsync(employee.Id, leaveType.Id, year))
                        continue;

                    // Calculate entitled days from policy
                    var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(employee.Id, leaveType.Id);

                    // Create balance
                    var balance = new EmployeeLeaveBalance
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        Year = year,
                        TotalEntitledDays = entitledDays,
                        UsedDays = 0
                    };

                    await _repository.AddAsync(balance);
                    createdCount++;
                }
            }

            return await _uow.CompleteAsync() > 0;
        }

        public async Task<bool> CreateBalanceAsync(EmployeeLeaveBalanceVM model)
        {
            // Validate: Check if balance already exists
            if (await BalanceExistsAsync(model.EmployeeId, model.LeaveTypeId, model.Year))
                return false;

            var balance = _mapper.Map<EmployeeLeaveBalance>(model);
            balance.UsedDays = 0; // Always start with 0 used days for manual creation

            await _repository.AddAsync(balance);
            return await _uow.CompleteAsync() > 0;
        }

        // ========== BALANCE UPDATES ==========

        public async Task<bool> UpdateBalanceEntitlementAsync(int balanceId, decimal newEntitledDays)
        {
            var balance = await _repository.GetByIdAsync(balanceId);
            if (balance == null) return false;

            balance.TotalEntitledDays = newEntitledDays;
            // RemainingDays will update automatically (computed column)

            _repository.Update(balance);
            return await _uow.CompleteAsync() > 0;
        }

        /// <summary>
        /// Updates UsedDays when a leave request is APPROVED.
        /// Called automatically from leave request approval process.
        /// </summary>
        public async Task<bool> UpdateUsedDaysAsync(int employeeId, int leaveTypeId, int year, decimal daysToAdd)
        {
            var balance = await GetBalanceAsync(employeeId, leaveTypeId, year);
            
            if (balance == null)
            {
                // Auto-create balance if it doesn't exist
                var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(employeeId, leaveTypeId);
                balance = new EmployeeLeaveBalance
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveTypeId,
                    Year = year,
                    TotalEntitledDays = entitledDays,
                    UsedDays = daysToAdd
                };
                await _repository.AddAsync(balance);
            }
            else
            {
                balance.UsedDays += daysToAdd;
                _repository.Update(balance);
            }

            return await _uow.CompleteAsync() > 0;
        }

        /// <summary>
        /// Reverses UsedDays when a leave request is REJECTED or DELETED.
        /// </summary>
        public async Task<bool> ReverseUsedDaysAsync(int employeeId, int leaveTypeId, int year, decimal daysToSubtract)
        {
            var balance = await GetBalanceAsync(employeeId, leaveTypeId, year);
            if (balance == null) return false;

            balance.UsedDays -= daysToSubtract;
            if (balance.UsedDays < 0) balance.UsedDays = 0; // Prevent negative

            _repository.Update(balance);
            return await _uow.CompleteAsync() > 0;
        }

        // ========== CARRY FORWARD ==========

        /// <summary>
        /// Carries forward ALL unused leave days from one year to the next.
        /// Runs for all employees and leave types.
        /// NO MAX LIMIT - carries forward full remaining balance.
        /// </summary>
        public async Task<bool> CarryForwardUnusedDaysAsync(int fromYear, int toYear)
        {
            // Get all balances from the previous year with remaining days > 0
            var balancesToCarryForward = await _repository
                .GetAllAsIQueryable()
                .Where(b => b.Year == fromYear && b.RemainingDays > 0)
                .ToListAsync();

            foreach (var oldBalance in balancesToCarryForward)
            {
                // Check if balance for next year already exists
                var nextYearBalance = await GetBalanceAsync(
                    oldBalance.EmployeeId, 
                    oldBalance.LeaveTypeId, 
                    toYear);

                if (nextYearBalance == null)
                {
                    // Create new balance for next year with carried forward days
                    var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(
                        oldBalance.EmployeeId, 
                        oldBalance.LeaveTypeId);

                    nextYearBalance = new EmployeeLeaveBalance
                    {
                        EmployeeId = oldBalance.EmployeeId,
                        LeaveTypeId = oldBalance.LeaveTypeId,
                        Year = toYear,
                        TotalEntitledDays = entitledDays + oldBalance.RemainingDays, // Add carried forward
                        UsedDays = 0
                    };

                    await _repository.AddAsync(nextYearBalance);
                }
                else
                {
                    // Balance exists, add remaining days to it
                    nextYearBalance.TotalEntitledDays += oldBalance.RemainingDays;
                    _repository.Update(nextYearBalance);
                }
            }

            return await _uow.CompleteAsync() > 0;
        }

        public async Task<bool> CarryForwardEmployeeBalancesAsync(int employeeId, int fromYear, int toYear)
        {
            var balances = await _repository
                .GetAllAsIQueryable()
                .Where(b => b.EmployeeId == employeeId && b.Year == fromYear && b.RemainingDays > 0)
                .ToListAsync();

            foreach (var oldBalance in balances)
            {
                var nextYearBalance = await GetBalanceAsync(employeeId, oldBalance.LeaveTypeId, toYear);

                if (nextYearBalance == null)
                {
                    var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(
                        employeeId, 
                        oldBalance.LeaveTypeId);

                    nextYearBalance = new EmployeeLeaveBalance
                    {
                        EmployeeId = employeeId,
                        LeaveTypeId = oldBalance.LeaveTypeId,
                        Year = toYear,
                        TotalEntitledDays = entitledDays + oldBalance.RemainingDays,
                        UsedDays = 0
                    };

                    await _repository.AddAsync(nextYearBalance);
                }
                else
                {
                    nextYearBalance.TotalEntitledDays += oldBalance.RemainingDays;
                    _repository.Update(nextYearBalance);
                }
            }

            return await _uow.CompleteAsync() > 0;
        }

        // ========== VALIDATION ==========

        /// <summary>
        /// Validates if employee has sufficient balance for a leave request.
        /// NO BORROWING FROM FUTURE - strictly enforces non-negative balance.
        /// </summary>
        public async Task<(bool IsValid, string? ErrorMessage)> ValidateLeaveRequestAsync(
            int employeeId, 
            int leaveTypeId, 
            int year, 
            decimal requestedDays)
        {
            var balance = await GetBalanceAsync(employeeId, leaveTypeId, year);

            if (balance == null)
            {
                // No balance exists - auto-create and validate
                var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(employeeId, leaveTypeId);
                
                if (requestedDays > entitledDays)
                    return (false, $"Insufficient balance. You have {entitledDays} days available, but requested {requestedDays} days.");
                
                return (true, null);
            }

            // Check if remaining balance is sufficient
            if (requestedDays > balance.RemainingDays)
            {
                return (false, $"Insufficient balance. You have {balance.RemainingDays} days remaining, but requested {requestedDays} days.");
            }

            return (true, null);
        }

        public async Task<bool> BalanceExistsAsync(int employeeId, int leaveTypeId, int year)
        {
            return await _repository
                .GetAllAsIQueryable()
                .AnyAsync(b => 
                    b.EmployeeId == employeeId && 
                    b.LeaveTypeId == leaveTypeId && 
                    b.Year == year);
        }

        /// <summary>
        /// Gets a balance by ID with all navigation properties loaded.
        /// </summary>
        public async Task<EmployeeLeaveBalance?> GetByIdWithDetailsAsync(int id)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Branch)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Department)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.Type)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.LeaveRequests)
                .Include(b => b.LeaveType)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
