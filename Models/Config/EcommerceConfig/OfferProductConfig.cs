using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OfferProductConfig : IEntityTypeConfiguration<OfferProduct>
    {
        public void Configure(EntityTypeBuilder<OfferProduct> builder)
        {
            builder.HasKey(op => new {op.OfferId,  op.ProductId});
        }
    }
}
