using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.UnitCost).HasPrecision(10,2);
            builder.Property(p => p.StandardPrice).HasPrecision(10,2);
            builder.Property(p => p.IsActive).HasDefaultValue(true);
            builder.Property(p => p.ReorderPoint).HasDefaultValue(0);
            builder.Property(p => p.LowStockAlert).HasDefaultValue(false);
            builder.Property(p => p.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.Property(p => p.ModifiedDate).HasDefaultValue(DateTime.Now);

        }
    }
}
