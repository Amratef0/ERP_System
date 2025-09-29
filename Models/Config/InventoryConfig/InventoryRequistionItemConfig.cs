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



            // chat solution many cascade paths

            builder.HasOne(i => i.Inventoryrequestion)                // current name in your class
                .WithMany(r => r.RequestedItems)                   // ensure InventoryRequisition.RequestedItems exists
                .HasForeignKey(i => i.RequestionId)
                .OnDelete(DeleteBehavior.Cascade);                 // cascade is OK here

            // Relationship -> Product (do NOT cascade; keep transactional history)
            builder.HasOne(i => i.Product)
                   .WithMany(p => p.InventoryRequestedProducts)       // ensure Product.InventoryRequestedProducts exists
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);               // avoid cascade path issues

            // Relationship -> ProductVariant (do NOT cascade)
            //builder.HasOne(i => i.ProductVariant)
            //       .WithMany(v => v.InventoryRequestedVariantProducts)         // ensure ProductVariant contains this nav or use .WithMany()
            //       .HasForeignKey(i => i.VariantId)
            //       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
