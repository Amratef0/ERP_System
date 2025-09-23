using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class EmployeeLeaveBalanceConfig : IEntityTypeConfiguration<EmployeeLeaveBalance>
    {
        public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
        {
            builder.HasIndex(elb => elb.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(elb => elb.TotalEntitledDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            builder.Property(elb => elb.UsedDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            builder.Property(elb => elb.RemainingDays)
                   .HasPrecision(5, 2)
                   .HasComputedColumnSql("[TotalEntitledDays] - [UsedDays]");

            builder.Property(elb => elb.IsDeleted)
                   .HasDefaultValue(false);

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
