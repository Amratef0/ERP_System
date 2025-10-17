using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class EmployeeLeaveBalanceConfig : IEntityTypeConfiguration<EmployeeLeaveBalance>
    {
        public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
        {
            // Index for soft delete filter
            builder.HasIndex(elb => elb.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            // UNIQUE CONSTRAINT: One balance per Employee + LeaveType + Year
            builder.HasIndex(elb => new { elb.EmployeeId, elb.LeaveTypeId, elb.Year })
                   .IsUnique()
                   .HasFilter("[IsDeleted] = 0")
                   .HasDatabaseName("IX_EmployeeLeaveBalance_Unique");

            // Precision for decimal fields
            builder.Property(elb => elb.TotalEntitledDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            builder.Property(elb => elb.UsedDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            // COMPUTED COLUMN: RemainingDays = TotalEntitledDays - UsedDays
            // This is always calculated, cannot be manually set
            builder.Property(elb => elb.RemainingDays)
                   .HasPrecision(5, 2)
                   .HasComputedColumnSql("[TotalEntitledDays] - [UsedDays]", stored: true);

            builder.Property(elb => elb.IsDeleted)
                   .HasDefaultValue(false);

            // Global query filter for soft delete
            builder.HasQueryFilter(elb => !elb.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(elb => elb.Employee)
                   .WithMany(e => e.EmployeeLeaveBalances)
                   .HasForeignKey(elb => elb.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(elb => elb.LeaveType)
                   .WithMany(lt => lt.EmployeeLeaveBalances)
                   .HasForeignKey(elb => elb.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
