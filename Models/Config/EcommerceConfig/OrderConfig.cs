using ERP_System_Project.Models.ECommerece;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.OrderNumber).IsUnique();
            builder.Property(o => o.TotalAmount).HasPrecision(15,4);
            builder.Property(o => o.SubTotalAmount).HasPrecision(15,4);
            builder.Property(o => o.TaxAmount).HasPrecision(15,4);
            builder.Property(o => o.ShippingAmount).HasPrecision(15,4);
            builder.Property(o => o.DiscountAmount).HasPrecision(15,4).HasDefaultValue(0);
            builder.Property(o => o.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.Property(o => o.ModifiedDate).HasDefaultValue(DateTime.Now);
        }
    }
}
