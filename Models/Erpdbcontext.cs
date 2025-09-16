using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ERP_System_Project.Models
{
    public class Erpdbcontext : IdentityDbContext<ApplicationUser>
    {
        public Erpdbcontext(DbContextOptions<Erpdbcontext> options) : base(options) { }


        #region Core
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        #endregion

        #region ECommerce
        public DbSet<Order> Orders { get; set; }

        #endregion

        #region Inventory
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<VariantAttributeValue> VariantAttributeValues { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ProductInventory> ProductsInventory { get; set; }
        public DbSet<InventoryTransactionType> InventoryTransactionTypes { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventoryRequisitionStatusCode> InventoryRequisitionStatusCodes { get; set; }
        public DbSet<InventoryRequisition> InventoryRequisitions { get; set; }
        public DbSet<InventoryRequisitionItem> InventoryRequisitionItems { get; set; }
        #endregion







        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Core
            builder.Entity<Currency>().HasIndex(c => c.Code).IsUnique();

            builder.Entity<Country>().HasIndex(c => c.Code).IsUnique();
            #endregion

            #region Inventory
            // Composite Key
            builder.Entity<VariantAttributeValue>().HasKey(vav => new { vav.VariantId, vav.AtrributeId });


            // unique attributes
            builder.Entity<ProductInventory>().HasIndex(pi => new { pi.ProductId, pi.WarehouseId }).IsUnique();
            builder.Entity<InventoryRequisition>().HasIndex(ir => ir.Number).IsUnique();

            #endregion

            #region HR
            builder.Entity<Employee>(e =>
            {
                e.HasIndex(e => e.Code).IsUnique();

                e.HasIndex(e => e.NationalId).IsUnique()
                    .HasFilter("[NationalId] IS NOT NULL");

                e.HasIndex(e => e.PassportNumber).IsUnique()
                    .HasFilter("[PassportNumber] IS NOT NULL");

                e.HasIndex(e => e.WorkEmail).IsUnique()
                    .HasFilter("[WorkEmail] IS NOT NULL");

                e.HasIndex(e => e.WorkPhone).IsUnique()
                    .HasFilter("[WorkPhone] IS NOT NULL");

                e.HasIndex(e => e.PersonalEmail).IsUnique()
                    .HasFilter("[PersonalEmail] IS NOT NULL");

                e.HasIndex(e => e.PersonalPhone).IsUnique()
                    .HasFilter("[PersonalPhone] IS NOT NULL");

                e.HasIndex(e => e.BankAccountNumber).IsUnique()
                    .HasFilter("[BankAccountNumber] IS NOT NULL");

                e.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                e.Property(e => e.ModifiedDate)
                      .HasDefaultValueSql("GETDATE()");
            });

            #endregion
        }
    }
}