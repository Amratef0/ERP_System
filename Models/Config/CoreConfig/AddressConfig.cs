using ERP_System_Project.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CoreConfig
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasIndex(a => a.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(a => a.IsActive).HasDefaultValue(true);

            builder.Property(a => a.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(a => !a.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(a => a.Country)
                   .WithMany(c => c.Addresses)
                   .HasForeignKey(a => a.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
