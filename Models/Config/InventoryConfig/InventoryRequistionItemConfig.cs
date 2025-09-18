using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class InventoryRequistionItemConfig : IEntityTypeConfiguration<InventoryRequisitionItem>
    {
        public void Configure(EntityTypeBuilder<InventoryRequisitionItem> builder)
        {
            builder.Property(iri => iri.QuantityRequested).HasPrecision(15, 4);
            builder.Property(iri => iri.QuantityApproved).HasPrecision(15, 4);
            builder.Property(iri => iri.CreatedDate).HasDefaultValue(DateTime.Now);
        }
    }
}
