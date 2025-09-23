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

            builder.HasIndex(e => e.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.BaseSalary)
                   .HasPrecision(15, 4);

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.ModifiedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(e => e.Branch)
                   .WithMany()
                   .HasForeignKey(e => e.BranchId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Type)
                   .WithMany(et => et.Employees)
                   .HasForeignKey(e => e.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobTitle)
                   .WithMany(jt => jt.Employees)
                   .HasForeignKey(e => e.JobTitleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Manager)
                   .WithMany()
                   .HasForeignKey(e => e.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.SalaryCurrency)
                   .WithMany()
                   .HasForeignKey(e => e.SalaryCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Address)
                   .WithMany()
                   .HasForeignKey(e => e.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
