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
        }
    }
}
