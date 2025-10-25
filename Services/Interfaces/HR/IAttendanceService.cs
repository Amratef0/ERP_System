using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IAttendanceService : IGenericService<AttendanceRecord>
    {
        Task<IEnumerable<EmployeeAttendanceRecordVM>> GetAllByDateAsync(
            DateOnly date,
            int countryId,
            string? name,
            int? branchId,
            int? departmentId,
            int? typeId,
            int? jobTitleId);

        Task<IEnumerable<EmployeeMonthlyAttendanceSummary>> GetMonthlyAttendanceSummaryAsync(
            int year,
            int month,
            int? countryId = null,
            int? branchId = null,
            int? departmentId = null,
            int? employeeTypeId = null,
            int? jobTitleId = null);
    }
}
