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
            builder.Property(it => it.UnitCost).HasPrecision(19, 4);
            builder.Property(it => it.TotalCost).HasPrecision(19, 4);
            builder.Property(it => it.CreatedDate).HasDefaultValue(DateTime.Now);
        }
    }
}
