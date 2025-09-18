using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class InventoryTransactionTypeConfig : IEntityTypeConfiguration<InventoryTransactionType>
    {
        public void Configure(EntityTypeBuilder<InventoryTransactionType> builder)
        {
            builder.Property(itt => itt.IsActive).HasDefaultValue(true);
        }
    }
}
