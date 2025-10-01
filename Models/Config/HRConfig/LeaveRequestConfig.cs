using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class LeaveRequestConfig : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasIndex(lr => lr.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(lr => lr.TotalDays)
                   .HasPrecision(5, 2);

            builder.Property(lr => lr.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(lr => lr.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(lr => !lr.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(lr => lr.Employee)
                   .WithMany(e => e.LeaveRequests)
                   .HasForeignKey(lr => lr.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.LeaveType)
                   .WithMany(lt => lt.LeaveRequests)
                   .HasForeignKey(lr => lr.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.ApprovedBy)
                   .WithMany(e => e.ApprovedTeamLeaveRequests)
                   .HasForeignKey(lr => lr.ApprovedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
