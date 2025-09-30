using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class EmployeeTypeConfig : IEntityTypeConfiguration<EmployeeType>
    {
        public void Configure(EntityTypeBuilder<EmployeeType> builder)
        {
            builder.HasIndex(et => et.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(et => et.IsActive)
                   .HasDefaultValue(true);

            builder.Property(et => et.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(et => !et.IsDeleted);

            builder.HasData(
                new EmployeeType
                {
                    Id = 1,
                    Name = "Full-Time",
                    Description = "Permanent employee working standard hours (e.g., 40 hours/week).",
                    IsActive = true,
                    IsDeleted = false
                },
                new EmployeeType
                {
                    Id = 2,
                    Name = "Part-Time",
                    Description = "Employee working fewer hours than a full-time employee.",
                    IsActive = true,
                    IsDeleted = false
                },
                new EmployeeType
                {
                    Id = 3,
                    Name = "Contractor",
                    Description = "Hired for a specific project or a defined period.",
                    IsActive = true,
                    IsDeleted = false
                },
                new EmployeeType
                {
                    Id = 4,
                    Name = "Intern",
                    Description = "A student or trainee gaining practical experience.",
                    IsActive = true,
                    IsDeleted = false
                },
                new EmployeeType
                {
                    Id = 5,
                    Name = "Freelancer",
                    Description = "Self-employed individual providing services to multiple clients.",
                    IsActive = true,
                    IsDeleted = false
                },
                new EmployeeType
                {
                    Id = 6,
                    Name = "Temporary",
                    Description = "Hired for a short-term need, often through an agency.",
                    IsActive = true,
                    IsDeleted = false
                }
            );
        }
    }
}
