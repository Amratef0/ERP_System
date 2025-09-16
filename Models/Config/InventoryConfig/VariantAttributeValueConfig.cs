using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class VariantAttributeValueConfig : IEntityTypeConfiguration<VariantAttributeValue>
    {
        public void Configure(EntityTypeBuilder<VariantAttributeValue> builder)
        {
            // Composite Key
            builder.HasKey(vav => new { vav.VariantId, vav.AtrributeId });
        }
    }
}
