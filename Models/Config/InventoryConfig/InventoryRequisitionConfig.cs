using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class InventoryRequisitionConfig : IEntityTypeConfiguration<InventoryRequisition>
    {
        public void Configure(EntityTypeBuilder<InventoryRequisition> builder)
        {
            builder.HasIndex(ir => ir.Number).IsUnique();

        }
    }
}
