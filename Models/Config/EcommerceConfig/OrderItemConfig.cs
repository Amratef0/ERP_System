using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Quantity).HasPrecision(15, 4);
            builder.Property(oi => oi.UnitPrice).HasPrecision(15, 4);
            builder.Property(oi => oi.DiscountAmount).HasPrecision(15, 4).HasDefaultValue(0);
            builder.Property(oi => oi.DiscountPercentage).HasPrecision(15, 4).HasDefaultValue(0);
            builder.Property(oi => oi.LineTotal).HasPrecision(15, 4);
            builder.Property(oi => oi.TaxAmount).HasPrecision(15, 4);
            builder.Property(oi => oi.CreatedDate).HasDefaultValueSql("GETDATE()");
        }
    } 
}
