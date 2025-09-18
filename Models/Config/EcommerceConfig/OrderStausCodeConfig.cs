using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OrderStatusCodeConfig : IEntityTypeConfiguration<OrderStatusCode>
    {
        public void Configure(EntityTypeBuilder<OrderStatusCode> builder)
        {
            builder.Property(osc => osc.IsActive).HasDefaultValue(true);
            builder.Property(osc => osc.Cost).HasPrecision(19,4);
        }
    }
}
