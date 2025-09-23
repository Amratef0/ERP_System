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
        }
    }
}
