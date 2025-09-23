using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class LeaveRequestStatusCodeConfig : IEntityTypeConfiguration<LeaveRequestStatusCode>
    {
        public void Configure(EntityTypeBuilder<LeaveRequestStatusCode> builder)
        {
            builder.HasIndex(lrsc => lrsc.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(lrsc => lrsc.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(lrsc => !lrsc.IsDeleted);
        }
    }
}