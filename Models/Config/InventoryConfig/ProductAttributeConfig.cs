using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class ProductAttributeConfig : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.Property(pa => pa.IsActive).HasDefaultValue(true);
            builder.Property(pa => pa.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.HasData(
               new ProductAttribute {Id = 1,Name = "Color", Type = "Text" },
               new ProductAttribute {Id = 2,Name = "Size", Type = "Text" }
            );
        }
    }
}
