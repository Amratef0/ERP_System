using ERP_System_Project.Models.Enums;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class PayrollCalculationService : IPayrollCalculationService
    {
        private readonly IUnitOfWork _uow;
        private const decimal OVERTIME_RATE_MULTIPLIER = 1.5m;
        private const decimal TAX_RATE = 0.10m; // 10%
        private const int STANDARD_WORK_HOURS_PER_DAY = 8;

        public PayrollCalculationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<decimal> CalculateOvertimeAmountAsync(int employeeId, int year, int month)
        {
            var employee = await _uow.Employees.GetByIdAsync(employeeId);
            if (employee == null) return 0;

            // Get total overtime hours from attendance records for the month
            var periodStart = new DateOnly(year, month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            var totalOvertimeHours = await _uow.AttendanceRecords
                .GetAllAsIQueryable()
                .Where(ar => ar.EmployeeId == employeeId &&
                            ar.Date >= periodStart &&
                            ar.Date <= periodEnd &&
                            ar.StatusCodeId == 1) // Present only
                .SumAsync(ar => ar.OverTimeHours);

            // Calculate hourly rate
            var standardMonthlyHours = await GetStandardMonthlyHoursAsync(employeeId, year, month);
            if (standardMonthlyHours == 0) return 0;

            var hourlyRate = employee.BaseSalary / standardMonthlyHours;
            var overtimeRate = hourlyRate * OVERTIME_RATE_MULTIPLIER;

            return totalOvertimeHours * overtimeRate;
        }

        public decimal CalculateTaxAmount(decimal grossSalary)
        {
            return Math.Round(grossSalary * TAX_RATE, 4);
        }

        public async Task<decimal> CalculateUnpaidLeaveDeductionAsync(int employeeId, int year, int month)
        {
            var employee = await _uow.Employees.GetByIdAsync(employeeId);
            if (employee == null) return 0;

            var periodStart = new DateOnly(year, month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            // Get unpaid leave type ID (LeaveType Code = "UNPAID_LEAVE")
            var unpaidLeaveType = await _uow.LeaveTypes
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(lt => lt.Code == "UNPAID_LEAVE");

            if (unpaidLeaveType == null) return 0;

            // Get approved unpaid leave requests for this month
            var unpaidLeaveDays = await _uow.LeaveRequests
                .GetAllAsIQueryable()
                .Where(lr => lr.EmployeeId == employeeId &&
                            lr.LeaveTypeId == unpaidLeaveType.Id &&
                            lr.Status == LeaveRequestStatus.Approved &&
                            lr.StartDate <= periodEnd &&
                            lr.EndDate >= periodStart)
                .SumAsync(lr => lr.TotalDays);

            if (unpaidLeaveDays == 0) return 0;

            // Calculate working days in month
            var workingDays = await GetWorkingDaysInMonthAsync(employeeId, year, month);
            if (workingDays == 0) return 0;

            // Deduction = (BaseSalary / WorkingDays) * UnpaidLeaveDays
            var dailySalary = employee.BaseSalary / workingDays;
            return Math.Round(dailySalary * unpaidLeaveDays, 4);
        }

        public async Task<decimal> CalculateBonusAmountAsync(int employeeId, int year, int month)
        {
            // Currently returns 0 - can be extended for performance-based bonuses
            // Future: Check attendance percentage, performance ratings, etc.
            return await Task.FromResult(0m);
        }

        public async Task<decimal> CalculateAllowanceAmountAsync(int employeeId, int year, int month)
        {
            // Currently returns 0 - can be extended for allowances
            // Future: Transportation, housing, meal allowances, etc.
            return await Task.FromResult(0m);
        }

        public async Task<int> GetWorkingDaysInMonthAsync(int employeeId, int year, int month)
        {
            var employee = await _uow.Employees
                .GetAllAsIQueryable()
                .Include(e => e.Branch)
                    .ThenInclude(b => b.Address)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null) return 22; // Default fallback

            var periodStart = new DateOnly(year, month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            // Count working days (Monday-Friday)
            int workingDays = 0;
            for (var date = periodStart; date <= periodEnd; date = date.AddDays(1))
            {
                // Skip Saturdays and Sundays
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    workingDays++;
            }

            // Subtract public holidays
            var publicHolidays = await _uow.PublicHolidays
                .GetAllAsIQueryable()
                .Where(ph => ph.CountryId == employee.Branch.Address.CountryId &&
                            ph.Date >= periodStart &&
                            ph.Date <= periodEnd)
                .CountAsync();

            workingDays -= publicHolidays;

            return workingDays > 0 ? workingDays : 22; // Fallback to 22
        }

        public async Task<decimal> GetStandardMonthlyHoursAsync(int employeeId, int year, int month)
        {
            var workingDays = await GetWorkingDaysInMonthAsync(employeeId, year, month);
            return workingDays * STANDARD_WORK_HOURS_PER_DAY;
        }
    }
}
