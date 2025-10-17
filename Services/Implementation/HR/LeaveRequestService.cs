using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class LeaveRequestService : GenericService<LeaveRequest>, ILeaveRequestService
    {
        private readonly IEmployeeLeaveBalanceService _leaveBalanceService;
        private readonly IPublicHolidayService _publicHolidayService;

        public LeaveRequestService(
            IUnitOfWork uow,
            IEmployeeLeaveBalanceService leaveBalanceService,
            IPublicHolidayService publicHolidayService) : base(uow)
        {
            _leaveBalanceService = leaveBalanceService;
            _publicHolidayService = publicHolidayService;
        }

        public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveRequestsAsync(int employeeId)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveRequestsByStatusAsync(int employeeId, LeaveRequestStatus status)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .Where(lr => lr.EmployeeId == employeeId && lr.Status == status)
                .OrderByDescending(lr => lr.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync(int? departmentId = null)
        {
            var query = _repository
                .GetAllAsIQueryable()
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Branch)
                .Include(lr => lr.LeaveType)
                .Where(lr => lr.Status == LeaveRequestStatus.Pending);

            if (departmentId.HasValue)
                query = query.Where(lr => lr.Employee.DepartmentId == departmentId.Value);

            return await query
                .OrderBy(lr => lr.CreatedDate)
                .ToListAsync();
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            // Validate dates
            if (leaveRequest.EndDate < leaveRequest.StartDate)
                return (false, "End date cannot be before start date.");

            // Calculate leave days
            var leaveDays = await CalculateLeaveDaysAsync(
                leaveRequest.StartDate,
                leaveRequest.EndDate,
                leaveRequest.EmployeeId);

            leaveRequest.TotalDays = leaveDays;

            // Validate balance
            var (isValid, errorMessage) = await _leaveBalanceService.ValidateLeaveRequestAsync(
                leaveRequest.EmployeeId,
                leaveRequest.LeaveTypeId,
                leaveRequest.StartDate.Year,
                leaveDays);

            if (!isValid)
                return (false, errorMessage);

            // Set initial status and created date
            leaveRequest.Status = LeaveRequestStatus.Pending;
            leaveRequest.CreatedDate = DateTime.UtcNow;

            await _repository.AddAsync(leaveRequest);
            var saved = await _uow.CompleteAsync();

            return saved > 0
                ? (true, null)
                : (false, "Failed to create leave request.");
        }

        public async Task<(bool Success, string? ErrorMessage)> ApproveLeaveRequestAsync(int leaveRequestId, int approvedById)
        {
            var leaveRequest = await GetByIdWithDetailsAsync(leaveRequestId);
            if (leaveRequest == null)
                return (false, "Leave request not found.");

            if (leaveRequest.Status != LeaveRequestStatus.Pending)
                return (false, "Leave request has already been processed.");

            // Update leave balance
            var balanceUpdated = await _leaveBalanceService.UpdateUsedDaysAsync(
                leaveRequest.EmployeeId,
                leaveRequest.LeaveTypeId,
                leaveRequest.StartDate.Year,
                leaveRequest.TotalDays);

            if (!balanceUpdated)
                return (false, "Failed to update leave balance.");

            // Update leave request status
            leaveRequest.Status = LeaveRequestStatus.Approved;
            leaveRequest.ApprovedById = approvedById;
            leaveRequest.ApprovedDate = DateTime.UtcNow;

            _repository.Update(leaveRequest);
            var saved = await _uow.CompleteAsync();

            return saved > 0
                ? (true, null)
                : (false, "Failed to approve leave request.");
        }

        public async Task<(bool Success, string? ErrorMessage)> RejectLeaveRequestAsync(int leaveRequestId, int approvedById)
        {
            var leaveRequest = await GetByIdWithDetailsAsync(leaveRequestId);
            if (leaveRequest == null)
                return (false, "Leave request not found.");

            if (leaveRequest.Status != LeaveRequestStatus.Pending)
                return (false, "Leave request has already been processed.");

            leaveRequest.Status = LeaveRequestStatus.Rejected;
            leaveRequest.ApprovedById = approvedById;
            leaveRequest.ApprovedDate = DateTime.UtcNow;

            _repository.Update(leaveRequest);
            var saved = await _uow.CompleteAsync();

            return saved > 0
                ? (true, null)
                : (false, "Failed to reject leave request.");
        }

        public async Task<(bool Success, string? ErrorMessage)> CancelLeaveRequestAsync(int leaveRequestId, int employeeId)
        {
            var leaveRequest = await GetByIdWithDetailsAsync(leaveRequestId);
            if (leaveRequest == null)
                return (false, "Leave request not found.");

            if (leaveRequest.EmployeeId != employeeId)
                return (false, "You can only cancel your own leave requests.");

            if (leaveRequest.Status == LeaveRequestStatus.Approved)
            {
                // Reverse the used days from balance
                await _leaveBalanceService.ReverseUsedDaysAsync(
                    leaveRequest.EmployeeId,
                    leaveRequest.LeaveTypeId,
                    leaveRequest.StartDate.Year,
                    leaveRequest.TotalDays);
            }

            leaveRequest.Status = LeaveRequestStatus.Cancelled;

            _repository.Update(leaveRequest);
            var saved = await _uow.CompleteAsync();

            return saved > 0
                ? (true, null)
                : (false, "Failed to cancel leave request.");
        }

        public async Task<LeaveRequest?> GetByIdWithDetailsAsync(int id)
        {
            return await _repository
                .GetAllAsIQueryable()
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Branch)
                .Include(lr => lr.Employee)
                    .ThenInclude(e => e.Department)
                .Include(lr => lr.LeaveType)
                .Include(lr => lr.ApprovedBy)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<decimal> CalculateLeaveDaysAsync(DateOnly startDate, DateOnly endDate, int employeeId)
        {
            if (endDate < startDate)
                return 0;

            decimal totalDays = 0;
            var currentDate = startDate;

            // Get public holidays for the year
            var publicHolidays = await _publicHolidayService.GetAllAsync();
            var holidays = publicHolidays
                .Where(h => h.Date.Year == startDate.Year || h.Date.Year == endDate.Year)
                .Select(h => h.Date)
                .ToList();

            while (currentDate <= endDate)
            {
                // Skip weekends (Saturday = 6, Sunday = 0)
                if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                    currentDate.DayOfWeek != DayOfWeek.Sunday &&
                    !holidays.Contains(currentDate))
                {
                    totalDays++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return totalDays;
        }
    }
}
