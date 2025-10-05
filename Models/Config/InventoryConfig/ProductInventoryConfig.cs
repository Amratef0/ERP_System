using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class ProductInventoryConfig : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            builder.HasIndex(pi => new { pi.ProductId, pi.WarehouseId }).IsUnique();
            builder.Property(pi => pi.QuantityCommitted).HasPrecision(15,4).HasDefaultValue(0);
            builder.Property(pi => pi.QuantityAvailable).HasPrecision(15,4).HasDefaultValue(0);
            builder.Property(pi => pi.QuantityOnOrder).HasPrecision(15,4).HasDefaultValue(0);
            builder.Property(pi => pi.QuantityOnHand).HasPrecision(15,4).HasDefaultValue(0);
            builder.Property(pi => pi.CreatedDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
