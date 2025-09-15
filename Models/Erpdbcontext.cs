using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Models
{
    public class Erpdbcontext : IdentityDbContext<ApplicationUser>
    {
        public Erpdbcontext(DbContextOptions<Erpdbcontext> options) : base(options) { }


        #region CoreTables
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        #endregion




        #region Ecommerce&Inventory
        public DbSet<Brand> Brands { get; set; } 
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; } 
        public DbSet<ProductType> Product_Types { get; set; } 
        public DbSet<UnitOfMeasure> Units_Of_Measure { get; set; }
        public DbSet<ProductAttribute> Product_Attributes { get; set; }
        public DbSet<ProductVariant> Product_Variants { get; set; }
        public DbSet<VariantAttributeValue> Variant_Attribute_Values { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ProductInventory> Product_Inventory { get; set; }
        public DbSet<InventoryTransactionType> Inventory_Transaction_Types { get; set; }
        #endregion







        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region CoreTables
            builder.Entity<Currency>().HasIndex(c => c.Currency_Code).IsUnique();

            builder.Entity<Country>().HasIndex(c => c.Country_Code).IsUnique();
            #endregion


            #region Ecommerce&Inventory
            // Composite Key
            builder.Entity<VariantAttributeValue>().HasKey(vav => new { vav.Variant_Id, vav.Atrribute_Id });


            // unique attributes
            builder.Entity<ProductInventory>().HasIndex(pi => new { pi.Product_Id, pi.Warehouse_Id }).IsUnique();

            #endregion
        }
    }
}