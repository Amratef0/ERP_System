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
                .HasColumnType("NVARCHAR(255)")
                .IsRequired();

            builder.Property(b => b.brand_description)
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired();

            builder.Property(b => b.logo_url)
                .HasColumnType("NVARCHAR(255)")
                .IsRequired();

            builder.Property(b => b.website_url)
                .HasColumnType("NVARCHAR(255)");


            // relationships

        }
    }
}
