using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class WorkScheduleDayConfig : IEntityTypeConfiguration<WorkScheduleDay>
    {
        public void Configure(EntityTypeBuilder<WorkScheduleDay> builder)
        {
            builder.HasData(
                new WorkScheduleDay { Id = 1, WorkScheduleId = 1, Day = Days.Sunday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 2, WorkScheduleId = 1, Day = Days.Monday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 3, WorkScheduleId = 1, Day = Days.Tuesday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 4, WorkScheduleId = 1, Day = Days.Wednesday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 5, WorkScheduleId = 1, Day = Days.Thursday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 6, WorkScheduleId = 1, Day = Days.Friday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null },
                new WorkScheduleDay { Id = 7, WorkScheduleId = 1, Day = Days.Saturday, IsWorkDay = false, WorkStartTime = null, WorkEndTime = null }
            );
        }
    }
}