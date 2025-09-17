using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class PayrollRunConfig : IEntityTypeConfiguration<PayrollRun>
    {
        public void Configure(EntityTypeBuilder<PayrollRun> builder)
        {
            builder.Property(pr => pr.IsLocked)
                   .HasDefaultValue(false);

            builder.Property(pr => pr.TotalAmount)
                   .HasPrecision(15, 4);

            builder.Property(pr => pr.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
