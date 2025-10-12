using Bogus;
using ERP_System_Project.Models;
using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERP_System_Project.Services.Implementation
{
    public class SeedService
    {
        private readonly Erpdbcontext _db;
        public SeedService(Erpdbcontext db) => _db = db;

        public async Task SeedAttendanceRecordsAsync(int count = 200, bool flag = false)
        {
            if (!flag) return;

            if (!await _db.Employees.AnyAsync())
            {
                Console.WriteLine("No Employees");
                return;
            }

            var employeeIds = _db.Employees.Select(u => u.Id).ToList();
            var statusIds = await _db.AttendanceStatusCodes.Select(s => s.Id).ToListAsync();

            var recordFaker = new Bogus.Faker<AttendanceRecord>()
               .RuleFor(r => r.EmployeeId, f => f.PickRandom(employeeIds))
               .RuleFor(r => r.StatusCodeId, f => f.PickRandom(statusIds))
               .RuleFor(r => r.CheckInTime, f => f.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now)
                                               .AddHours(f.Random.Int(8, 10))
                                               .AddMinutes(f.Random.Int(0, 59)))
               .RuleFor(r => r.CheckOutTime, (f, r) => r.CheckInTime.AddHours(f.Random.Double(7, 10)))
               .RuleFor(r => r.Date, (f, r) => DateOnly.FromDateTime(r.CheckInTime))
               .RuleFor(r => r.TotalHours, (f, r) => (decimal)((r.CheckOutTime!.Value - r.CheckInTime).TotalHours))
               .RuleFor(r => r.OverTimeHours, (f, r) => r.TotalHours > 8 ? r.TotalHours - 8 : 0)
               .RuleFor(r => r.Notes, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence(5) : null)
               .RuleFor(r => r.CreatedDate, f => f.Date.Recent(30));

            var attendanceRecords = recordFaker.Generate(count);

            await _db.AttendanceRecords.AddRangeAsync(attendanceRecords);
            await _db.SaveChangesAsync();
        }
    }
}
