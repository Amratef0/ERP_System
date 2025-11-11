using ERP_System_Project.Models.ECommerece;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.TotalAmount).HasPrecision(15,4);
            builder.Property(o => o.TaxAmount).HasPrecision(15,4);
            builder.Property(o => o.ShippingAmount).HasPrecision(15,4);
            builder.Property(o => o.OrderDate).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();


            builder.HasOne(o => o.PaymentMethodType)
                .WithMany(pmt => pmt.Orders)
                .HasForeignKey(o => o.PaymentMethodTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.BillingAddress)
                   .WithMany(a => a.OrderBillingAddresses)
                   .HasForeignKey(o => o.BillingAddressId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
