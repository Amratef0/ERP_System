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
            builder.Property(o => o.CreatedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.Property(o => o.ModifiedDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();




            builder.HasOne(o => o.ShippingAddress)
                  .WithMany(a => a.OrderShippingAddresses)
                  .HasForeignKey(o => o.ShippingAddressId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.BillingAddress)
                   .WithMany(a => a.OrderBillingAddresses)
                   .HasForeignKey(o => o.BillingAddressId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
