using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasIndex(d => d.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(d => d.IsActive)
                   .HasDefaultValue(true);

            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(d => !d.IsDeleted);

            builder.HasData(
            new Department
            {
                Id = 1,
                Name = "Human Resources",
                Code = "HR",
                CostCenterCode = "CC-HR-01"
            },
            new Department
            {
                Id = 2,
                Name = "Customer Relationship Management",
                Code = "CRM",
                CostCenterCode = "CC-CRM-01"
            },
            new Department
            {
                Id = 3,
                Name = "Inventory",
                Code = "INV",
                CostCenterCode = "CC-INV-01"
            },
            new Department
            {
                Id = 4,
                Name = "Accounting & Finance",
                Code = "FIN",
                CostCenterCode = "CC-FIN-01"
            },
            new Department
            {
                Id = 5,
                Name = "Supplying & Purchasing",
                Code = "PUR",
                CostCenterCode = "CC-PUR-01"
            }
        );
        }
    }
}
