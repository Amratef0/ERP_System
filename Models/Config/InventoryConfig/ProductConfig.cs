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
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            


           builder.HasMany(p=> p.CustomerFavorites)
                .WithOne(f=>f.Product)
                .HasForeignKey(f=>f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(p => p.CustomerWishlists)
                   .WithOne(w => w.Product)
                   .HasForeignKey(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.CustomerReviews)
                   .WithOne(r => r.Product)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Category)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
