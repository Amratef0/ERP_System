using ERP_System_Project.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Configuration
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            // properties constraints
            builder.HasKey(b => b.brand_Id);

            builder.Property(b => b.brand_name)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(b => b.brand_description)
                .IsRequired();

            builder.Property(b => b.logo_url)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(b => b.website_url)
                .HasMaxLength(255)
                .IsRequired(false);


            // relationships

        }
    }
}
