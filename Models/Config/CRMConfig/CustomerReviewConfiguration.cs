using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CRMConfig
{
    public class CustomerReviewConfiguration : IEntityTypeConfiguration<CustomerReview>
    {
        public void Configure(EntityTypeBuilder<CustomerReview> builder)
        {

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title).HasMaxLength(200).IsRequired();
            builder.Property(r => r.Description).HasMaxLength(2000).IsRequired();
            builder.Property(r => r.Rating).IsRequired();


            builder.Property(c => c.CreatedAt)
                 .HasDefaultValueSql("GETDATE()")
                 .ValueGeneratedOnAdd()
                 .IsRequired();
        }
    }
}
