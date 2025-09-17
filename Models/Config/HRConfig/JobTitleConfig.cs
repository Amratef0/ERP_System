using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class JobTitleConfig : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.Property(jt => jt.MinSalary)
                   .HasPrecision(15, 4);

            builder.Property(jt => jt.MaxSalary)
                   .HasPrecision(15, 4);

            builder.Property(jt => jt.IsActive)
                   .HasDefaultValue(true);
        }
    }
}
