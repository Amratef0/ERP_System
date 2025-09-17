using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OfferTypeConfig : IEntityTypeConfiguration<OfferType>
    {
        public void Configure(EntityTypeBuilder<OfferType> builder)
        {
            builder.HasIndex(ot => ot.Code).IsUnique();
        }
    }
}
