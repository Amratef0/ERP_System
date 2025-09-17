using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class LeaveRequestConfig : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.Property(lr => lr.TotalDays)
                   .HasPrecision(5, 2);

            builder.Property(lr => lr.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
