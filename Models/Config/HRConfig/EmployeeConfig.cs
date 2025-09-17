using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasIndex(e => e.Code)
                   .IsUnique();

            builder.HasIndex(e => e.NationalId).IsUnique()
                   .HasFilter("[NationalId] IS NOT NULL");

            builder.HasIndex(e => e.PassportNumber).IsUnique()
                   .HasFilter("[PassportNumber] IS NOT NULL");

            builder.HasIndex(e => e.WorkEmail).IsUnique()
                   .HasFilter("[WorkEmail] IS NOT NULL");

            builder.HasIndex(e => e.WorkPhone).IsUnique()
                   .HasFilter("[WorkPhone] IS NOT NULL");

            builder.HasIndex(e => e.PersonalEmail).IsUnique()
                   .HasFilter("[PersonalEmail] IS NOT NULL");

            builder.HasIndex(e => e.PersonalPhone).IsUnique()
                   .HasFilter("[PersonalPhone] IS NOT NULL");

            builder.HasIndex(e => e.BankAccountNumber).IsUnique()
                   .HasFilter("[BankAccountNumber] IS NOT NULL");

            builder.Property(e => e.BaseSalary)
                   .HasPrecision(15, 4);

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.ModifiedDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
