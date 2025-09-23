using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class WorkScheduleDayConfig : IEntityTypeConfiguration<WorkScheduleDay>
    {
        public void Configure(EntityTypeBuilder<WorkScheduleDay> builder)
        {
            builder.HasIndex(wsd => wsd.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(wsd => wsd.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(wsd => !wsd.IsDeleted);
        }
    }
}