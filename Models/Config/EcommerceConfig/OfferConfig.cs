using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.Property(o => o.IsActive).HasDefaultValue(true);
            builder.Property(o => o.DiscountValue).HasPrecision(10, 2);
            builder.Property(o => o.MinOrderAmount).HasPrecision(15, 2);
        }
    }
}
