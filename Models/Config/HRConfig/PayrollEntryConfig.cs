using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class PayrollEntryConfig : IEntityTypeConfiguration<PayrollEntry>
    {
        public void Configure(EntityTypeBuilder<PayrollEntry> builder)
        {
            builder.Property(pe => pe.BaseSalaryAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.OvertimeAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.BonusAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.AllowanceAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.DeductionAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.TaxAmount)
                   .HasPrecision(15, 4)
                   .HasDefaultValue(0);

            builder.Property(pe => pe.NetAmount)
                   .HasPrecision(15, 4)
                   .HasComputedColumnSql("[BaseSalaryAmount] + [OvertimeAmount] + [BonusAmount] + [AllowanceAmount] - [DeductionAmount] - [TaxAmount]");
        }
    }
}
