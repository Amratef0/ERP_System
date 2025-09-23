using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class AttendanceRecordConfig : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.HasIndex(ar => ar.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(ar => ar.ScheduledHours)
                   .HasPrecision(5, 2);

            builder.Property(ar => ar.TotalHours)
                   .HasPrecision(5, 2);

            builder.Property(p => p.OverTimeHours)
                   .HasPrecision(5, 2);

            builder.Property(ar => ar.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(ar => ar.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(ar => !ar.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(ar => ar.Employee)
                   .WithMany()
                   .HasForeignKey(ar => ar.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.StatusCode)
                   .WithMany(sc => sc.AttendanceRecords)
                   .HasForeignKey(ar => ar.StatusCodeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
