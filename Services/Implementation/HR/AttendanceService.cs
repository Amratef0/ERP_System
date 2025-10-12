using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.HR;
using Hangfire;
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

        [DisableConcurrentExecution(60)]
        public async Task GenerateDailyAttendance()
        {
            // Dont forget country


            //DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //var employees = await _uow.Employees.Where(e => e.IsActive).ToListAsync();

            //foreach (var emp in employees)
            //{
            //    // شيّك هل في سجل بالفعل للنهارده
            //    var existingRecord = await _repository.FirstOrDefaultAsync(a => a.EmployeeId == emp.Id && a.Date == today);

            //    // لو اليوم عطلة عامة
            //    bool isPublicHoliday = await _uow.PublicHolidays.AnyAsync(h => h.Date == today);

            //    // لو الموظف عنده إجازة
            //    bool isOnLeave = await _context.Leaves.AnyAsync(l =>
            //        l.EmployeeId == emp.Id && l.Date == today && l.Status == "Approved");

            //    // احسب الحالة الجديدة المفترض تكون
            //    string newStatus = isPublicHoliday ? "Public Holiday"
            //                     : isOnLeave ? "On Leave"
            //                     : "Pending";

            //    // لو السجل مش موجود → أضفه
            //    if (existingRecord == null)
            //    {
            //        _context.AttendanceRecords.Add(new AttendanceRecord
            //        {
            //            EmployeeId = emp.Id,
            //            Date = today,
            //            Status = newStatus
            //        });
            //    }
            //    else
            //    {
            //        // لو الحالة اتغيّرت → حدّثها
            //        if (existingRecord.Status != newStatus)
            //        {
            //            existingRecord.Status = newStatus;
            //            existingRecord.LastUpdated = DateTime.Now; // لو عندك كولمن للتعديل
            //            _context.AttendanceRecords.Update(existingRecord);
            //        }
            //    }
            //}
        }

        public async Task<IEnumerable<EmployeeAttendanceRecordVM>> GetAllByDateAsync(
            DateOnly date,
            int countryId,
            int? branchId,
            int? departmentId,
            int? typeId,
            int? jobTitleId)
        {
            var query = _repository.GetAllAsIQueryable();

            query = query.Include(ar => ar.Employee)
                            .ThenInclude(e => e.Branch)
                                .ThenInclude(b => b.Address)
                         .Include(ar => ar.StatusCode)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Branch)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Department)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.Type)
                         .Include(ar => ar.Employee)
                            .ThenInclude(e => e.JobTitle)
                         .Where(ar => ar.Date == date && ar.Employee.Branch.Address.CountryId == countryId);

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
