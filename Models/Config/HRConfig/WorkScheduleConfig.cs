using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class WorkScheduleConfig : IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
        {
            builder.HasIndex(ws => ws.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(ws => ws.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(ws => !ws.IsDeleted);

            builder.HasData(
                new WorkSchedule
                {
                    Id = 1,
                    Name = "Official Work Schedule",
                    Description = "This is the basic work schedule that applies to all employees.",
                    IsDeleted = false
                }
            );
        }
    }
}
