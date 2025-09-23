using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class PayrollEntryConfig : IEntityTypeConfiguration<PayrollEntry>
    {
        public void Configure(EntityTypeBuilder<PayrollEntry> builder)
        {
            builder.HasIndex(pe => pe.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

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

            builder.Property(pe => pe.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(pe => !pe.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(pe => pe.PayrollRun)
                   .WithMany(pr => pr.PayrollEntries)
                   .HasForeignKey(pe => pe.PayrollRunId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pe => pe.Employee)
                   .WithMany(e => e.PayrollEntries)
                   .HasForeignKey(pe => pe.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pe => pe.Currency)
                   .WithMany(c => c.PayrollEntries)
                   .HasForeignKey(pe => pe.CurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
