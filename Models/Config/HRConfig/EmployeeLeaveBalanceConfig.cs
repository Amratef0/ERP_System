using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class EmployeeLeaveBalanceConfig : IEntityTypeConfiguration<EmployeeLeaveBalance>
    {
        public void Configure(EntityTypeBuilder<EmployeeLeaveBalance> builder)
        {
            builder.Property(elb => elb.TotalEntitledDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            builder.Property(elb => elb.UsedDays)
                   .HasPrecision(5, 2)
                   .HasDefaultValue(0);

            builder.Property(elb => elb.RemainingDays)
                   .HasPrecision(5, 2)
                   .HasComputedColumnSql("[TotalEntitledDays] - [UsedDays]");
        }
    }
}
