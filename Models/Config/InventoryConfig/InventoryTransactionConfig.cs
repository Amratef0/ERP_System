using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class InventoryTransactionConfig : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.Property(it => it.Quantity).HasPrecision(19, 4);
          




            // chat gpt solution 
            // restrict to avoid cascade delete cycle 
            // implement soft delete better

            builder.HasOne(t => t.Product)
                .WithMany(p => p.InventoryTransactions)
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(t => t.Variant)
            //       .WithMany(v => v.InventoryTransactions)
            //       .HasForeignKey(t => t.VariantId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(t => t.Warehouse)
            //       .WithMany(w => w.InventoryTransactions)
            //       .HasForeignKey(t => t.WarehouseId)
            //       .OnDelete(DeleteBehavior.Restrict);

         
        }
    }
}
