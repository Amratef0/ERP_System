using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OfferCategoryConfig : IEntityTypeConfiguration<OfferCategory>
    {
        public void Configure(EntityTypeBuilder<OfferCategory> builder)
        {
            builder.HasKey(oc => new { oc.OfferId, oc.CategoryId });
        }
    }
}
