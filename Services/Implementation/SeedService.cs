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

        public async Task SeedAttendanceRecordsAsync(bool flag = false, int count = 200)
        {
            if (!flag) return;

            if (!await _db.Employees.AnyAsync())
            {
                Console.WriteLine("No Employees");
                return;
            }

            var employeeIds = _db.Employees.Select(u => u.Id).ToList();
            var statusIds = await _db.AttendanceStatusCodes.Select(s => s.Id).ToListAsync();

            // Preload public holiday dates and global day-off days
            var holidayDates = await _db.PublicHolidays.Select(h => h.Date).ToListAsync();
            var dayOffDays = await _db.WorkScheduleDays
                                      .Where(d => !d.IsWorkDay)
                                      .Select(d => d.Day)
                                      .ToListAsync();

            // Find the Absent status code id (prefer code "A")
            var absentStatusId = await _db.AttendanceStatusCodes
                                          .Where(c => c.Code == "A" || c.Name == "Absent")
                                          .Select(c => c.Id)
                                          .FirstOrDefaultAsync();

            var recordFaker = new Bogus.Faker<AttendanceRecord>()
               .RuleFor(r => r.EmployeeId, f => f.PickRandom(employeeIds))
               .RuleFor(r => r.Date, f => DateOnly.FromDateTime(f.Date.Between(DateTime.Now.AddMonths(-1), DateTime.Now)))
               .RuleFor(r => r.StatusCodeId, (f, r) =>
               {
                   bool isHoliday = holidayDates.Contains(r.Date);
                   bool isDayOff = dayOffDays.Contains(r.Date.DayOfWeek);
                   if ((isHoliday || isDayOff) && absentStatusId > 0)
                   {
                       return absentStatusId;
                   }
                   return f.PickRandom(statusIds);
               })
               .RuleFor(r => r.CheckInTime, (f, r) =>
               {
                   if (r.StatusCodeId == absentStatusId)
                       return null;
                   return new TimeOnly(f.Random.Int(8, 10), f.Random.Int(0, 59));
               })
               .RuleFor(r => r.CheckOutTime, (f, r) =>
               {
                   if (r.StatusCodeId == absentStatusId)
                       return null;
                   return r.CheckInTime!.Value.AddHours(f.Random.Double(7, 10));
               })
               .RuleFor(r => r.TotalHours, (f, r) =>
               {
                   if (r.CheckInTime == null || r.CheckOutTime == null)
                       return 0m;
                   return (decimal)((r.CheckOutTime!.Value - r.CheckInTime.Value).TotalHours);
               })
               .RuleFor(r => r.OverTimeHours, (f, r) => r.TotalHours > 8 ? r.TotalHours - 8 : 0)
               .RuleFor(r => r.Notes, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence(5) : null)
               .RuleFor(r => r.CreatedDate, f => f.Date.Recent(30));

            var attendanceRecords = recordFaker.Generate(count);

            await _db.AttendanceRecords.AddRangeAsync(attendanceRecords);
            await _db.SaveChangesAsync();
        }
    }
}
