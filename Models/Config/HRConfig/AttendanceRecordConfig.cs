using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class AttendanceRecordConfig : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.Property(ar => ar.ScheduledHours)
                   .HasPrecision(5, 2);

            builder.Property(ar => ar.TotalHours)
                   .HasPrecision(5, 2);

            builder.Property(p => p.OverTimeHours)
                   .HasPrecision(5, 2);

            builder.Property(ar => ar.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
