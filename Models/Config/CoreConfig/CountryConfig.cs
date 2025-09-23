using ERP_System_Project.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CoreConfig
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasIndex(c => c.Code).IsUnique();

            builder.HasIndex(c => c.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
