using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class PaymentStatusCodeConfig : IEntityTypeConfiguration<PaymentStatusCode>
    {
        public void Configure(EntityTypeBuilder<PaymentStatusCode> builder)
        {
            builder.Property(psc => psc.IsActive).HasDefaultValue(true);
        }
    }
}
