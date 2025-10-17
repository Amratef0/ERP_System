using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class AttendanceService : GenericService<AttendanceRecord>, IAttendanceService
    {
        private readonly IMapper mapper;

        public AttendanceService(IUnitOfWork uow, IMapper mapper) : base(uow)
        {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeAttendanceRecordVM>> GetAllByDateAsync(
            DateOnly date,
            int countryId,
            string? name,
            int? branchId,
            int? departmentId,
            int? typeId,
            int? jobTitleId)
        {
            var query = _repository.GetAllAsIQueryable();

            query = query.Include(ar => ar.Employee)
                            .ThenInclude(e => e.Branch)
                                .ThenInclude(b => b.Address)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Department)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Type)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.JobTitle)
                         .Include(ar => ar.StatusCode)
                         .Where(ar => ar.Date == date && ar.Employee.Branch.Address.CountryId == countryId);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(ar => (ar.Employee.FirstName + " " + ar.Employee.LastName).ToUpper().StartsWith(name.ToUpper()));
            }

            if (branchId.HasValue && branchId > 0)
            {
                query = query.Where(ar => ar.Employee.BranchId == branchId.Value);
            }

            if (departmentId.HasValue && departmentId > 0)
            {
                query = query.Where(ar => ar.Employee.DepartmentId == departmentId.Value);
            }

            if (typeId.HasValue && typeId > 0)
            {
                query = query.Where(ar => ar.Employee.TypeId == typeId.Value);
            }

            if (jobTitleId.HasValue && jobTitleId > 0)
            {
                query = query.Where(ar => ar.Employee.JobTitleId == jobTitleId.Value);
            }

            IEnumerable<AttendanceRecord> attendanceRecords = await query.ToListAsync();

            IEnumerable<EmployeeAttendanceRecordVM> attendanceRecordVMs = mapper.Map<IEnumerable<EmployeeAttendanceRecordVM>>(attendanceRecords);

            return attendanceRecordVMs;
        }
    }
}
