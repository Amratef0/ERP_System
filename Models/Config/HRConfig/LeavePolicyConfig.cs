using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class LeavePolicyConfig : IEntityTypeConfiguration<LeavePolicy>
    {
        public void Configure(EntityTypeBuilder<LeavePolicy> builder)
        {
            // Index for soft delete filter
            builder.HasIndex(lp => lp.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            // Composite index for efficient policy lookup
            builder.HasIndex(lp => new { lp.LeaveTypeId, lp.JobTitleId, lp.EmployeeTypeId, lp.IsActive })
                   .HasFilter("[IsDeleted] = 0");

            // Precision for decimal fields
            builder.Property(lp => lp.EntitledDays)
                   .HasPrecision(5, 2);

            // Default values
            builder.Property(lp => lp.IsActive)
                   .HasDefaultValue(true);

            builder.Property(lp => lp.IsDeleted)
                   .HasDefaultValue(false);

            builder.Property(lp => lp.Priority)
                   .HasDefaultValue(0);

            // Global query filter for soft delete
            builder.HasQueryFilter(lp => !lp.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(lp => lp.LeaveType)
                   .WithMany()
                   .HasForeignKey(lp => lp.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lp => lp.JobTitle)
                   .WithMany()
                   .HasForeignKey(lp => lp.JobTitleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lp => lp.EmployeeType)
                   .WithMany()
                   .HasForeignKey(lp => lp.EmployeeTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Seed data: Example leave policies
            builder.HasData(
                // Annual Leave Policies by Job Title
                new LeavePolicy
                {
                    Id = 1,
                    LeaveTypeId = 1, // Annual Leave
                    JobTitleId = null, // Default for all
                    EmployeeTypeId = null,
                    EntitledDays = 21,
                    Priority = 0,
                    IsActive = true,
                    Description = "Default annual leave for all employees"
                }
                // Add more seed data as needed based on your LeaveType and JobTitle IDs
            );
        }
    }
}
