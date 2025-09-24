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
            builder.Property(ir => ir.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.Property(ir => ir.ModifiedDate).HasDefaultValue(DateTime.Now);




            // chat gpt fixing migration error (the relation is not configured)
            builder.HasOne(r => r.RequestingEmployee)
                  .WithMany(e => e.RequestedRequisitions)
                  .HasForeignKey(r => r.RequestedByEmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ApprovingEmployee)
                   .WithMany(e => e.ApprovedRequisitions)
                   .HasForeignKey(r => r.ApprovedByEmployeeId)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
