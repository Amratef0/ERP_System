using ERP_System_Project.Models.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.LogsConfig
{
    public class PerformanceLogConfig : IEntityTypeConfiguration<PerformanceLog>
    {
        public void Configure(EntityTypeBuilder<PerformanceLog> builder)
        {
            builder.Property(pl => pl.Status).HasConversion<string>();
            builder.Property(pl => pl.RequestDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
