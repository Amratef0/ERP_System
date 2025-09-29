using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class ProductAttributeValueConfig : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            // Composite Key
            builder.HasKey(vav => new { vav.ProductId, vav.AtrributeId });
        }
    }
}
