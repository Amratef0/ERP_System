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

            // Build a set of days to seed within the last month
            var startDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(-1));
            var endDate = DateOnly.FromDateTime(DateTime.Today);

            var allDays = new List<DateOnly>();
            for (var d = startDate; d <= endDate; d = d.AddDays(1))
            {
                allDays.Add(d);
            }

            // Randomly pick up to 'count' days (or all if count >= total days)
            var faker = new Bogus.Faker();
            var daysToSeed = count >= allDays.Count ? allDays : faker.Random.ListItems(allDays, Math.Min(count, allDays.Count));

            var recordsToAdd = new List<AttendanceRecord>();

            foreach (var day in daysToSeed)
            {
                foreach (var employeeId in employeeIds)
                {
                    // Skip if a record already exists for this employee/day
                    bool exists = await _db.AttendanceRecords.AnyAsync(ar => ar.EmployeeId == employeeId && ar.Date == day);
                    if (exists) continue;

                    bool isHoliday = holidayDates.Contains(day);
                    bool isDayOff = dayOffDays.Contains(day.DayOfWeek);
                    int statusId;
                    TimeOnly? checkIn = null;
                    TimeOnly? checkOut = null;
                    decimal totalHours = 0m;
                    decimal overtime = 0m;
                    string? notes = null;

                    if ((isHoliday || isDayOff) && absentStatusId > 0)
                    {
                        statusId = absentStatusId;
                        // Set a non-null default check-in to avoid null issues; keep checkout null
                        checkIn = new TimeOnly(0, 0);
                        notes = isHoliday ? "Public Holiday" : "Day Off";
                    }
                    else
                    {
                        statusId = faker.PickRandom(statusIds);
                        checkIn = new TimeOnly(faker.Random.Int(8, 10), faker.Random.Int(0, 59));
                        checkOut = checkIn.Value.AddHours(faker.Random.Double(7, 10));
                        totalHours = (decimal)((checkOut.Value - checkIn.Value).TotalHours);
                        overtime = totalHours > 8 ? totalHours - 8 : 0;
                        notes = faker.Random.Bool(0.2f) ? faker.Lorem.Sentence(5) : null;
                    }

                    recordsToAdd.Add(new AttendanceRecord
                    {
                        EmployeeId = employeeId,
                        Date = day,
                        StatusCodeId = statusId,
                        CheckInTime = checkIn,
                        CheckOutTime = checkOut,
                        TotalHours = totalHours,
                        OverTimeHours = overtime,
                        Notes = notes,
                        CreatedDate = faker.Date.Recent(30)
                    });
                }
            }

            if (recordsToAdd.Count > 0)
            {
                await _db.AttendanceRecords.AddRangeAsync(recordsToAdd);
            }
            await _db.SaveChangesAsync();
        }
    }
}
