using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class AttendanceStatusCodeConfig : IEntityTypeConfiguration<AttendanceStatusCode>
    {
        public void Configure(EntityTypeBuilder<AttendanceStatusCode> builder)
        {
            builder.HasIndex(asc => asc.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(asc => asc.IsActive)
                   .HasDefaultValue(true);

            builder.Property(asc => asc.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(asc => !asc.IsDeleted);

            builder.HasData(
                new AttendanceStatusCode
                {
                    Id = 1,
                    Name = "Present",
                    Code = "P",
                    Description = "Employee is present for work.",
                    IsActive = true
                },
                new AttendanceStatusCode
                {
                    Id = 2,
                    Name = "Absent",
                    Code = "A",
                    Description = "Unexcused absence from work.",
                    IsActive = true
                },
                new AttendanceStatusCode
                {
                    Id = 3,
                    Name = "Sick Leave",
                    Code = "SL",
                    Description = "Excused absence due to illness.",
                    IsActive = true
                },
                new AttendanceStatusCode
                {
                    Id = 4,
                    Name = "Paid Leave",
                    Code = "PL",
                    Description = "Excused absence for vacation or personal time.",
                    IsActive = true
                },
                new AttendanceStatusCode
                {
                    Id = 5,
                    Name = "Public Holiday",
                    Code = "H",
                    Description = "Official public holiday.",
                    IsActive = true
                },
                new AttendanceStatusCode
                {
                    Id = 6,
                    Name = "Day Off",
                    Code = "OFF",
                    Description = "Scheduled non-working day (e.g., weekend).",
                    IsActive = true
                }
            );
        }
    }
}
