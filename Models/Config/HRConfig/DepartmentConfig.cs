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
        }
    }
}
