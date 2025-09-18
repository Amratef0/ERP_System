using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.InventoryConfig
{
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(b => b.CreatedDate).HasDefaultValue(DateTime.Now);
            builder.Property(b => b.IsActive).HasDefaultValue(true);
        }
    }
}
