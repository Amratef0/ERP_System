using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class ProductRatingConfig : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder.Property(pr => pr.RateDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
