using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class ProductVariantConfig : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.Property(pv => pv.AdditionalPrice).HasPrecision(10, 2).HasDefaultValue(0);
            builder.Property(pv => pv.Quantity).HasPrecision(10, 2).HasDefaultValue(0);
            builder.Property(pv => pv.ReorderPoint).HasPrecision(10, 2).HasDefaultValue(0);
            builder.Property(pv => pv.MinStockLevel).HasPrecision(10, 2);
            builder.Property(pv => pv.MaxStockLevel).HasPrecision(10, 2);
            builder.Property(pv => pv.LowStockAlert).HasDefaultValue(false);
            builder.Property(pv => pv.IsDefault).HasDefaultValue(false);
            builder.Property(pv => pv.IsActive).HasDefaultValue(true);
            builder.Property(pv => pv.CreatedDate).HasDefaultValue(DateTime.Now);
        }
    }
}
