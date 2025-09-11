using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Models
{
    public class Erpdbcontext : IdentityDbContext<ApplicationUser>
    {
        public Erpdbcontext(DbContextOptions<Erpdbcontext> options) : base(options) { }



        #region Ecommerce&Inventory
        public DbSet<Brand> Brands { get; set; } 
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductType> Product_Types { get; set; } 
        public DbSet<UnitOfMeasure> Units_Of_Measure { get; set; }
        public DbSet<ProductAttribute> Product_Attributes { get; set; }
        public DbSet<ProductVariant> Product_Variants { get; set; }
        public DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }
        public DbSet<Order> Orders { get; set; }
        #endregion







        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Ecommerce&Inventory
            // Composite Key
            builder.Entity<VariantAttributeValue>().HasKey(vav => new { vav.Variant_Id, vav.Atrribute_Id });

            #endregion
        }
    }
}