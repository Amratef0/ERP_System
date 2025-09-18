using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class PaymentMethodTypeConfig : IEntityTypeConfiguration<PaymentMethodType>
    {
        public void Configure(EntityTypeBuilder<PaymentMethodType> builder)
        {
            builder.Property(pmt => pmt.IsActive).HasDefaultValue(true);
        }
    }
}
