using ERP_System_Project.Models.Config.HRConfig;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Inventory;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        #region CRM
        public DbSet<Customer> Customers{ get; set; }
        public DbSet<CustomerAddress> CustomerAddresses{ get; set; }
        public DbSet<CustomerFavorite>  CustomerFavorites{ get; set; }
        public DbSet<CustomerReview> CustomerReviews{ get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<CustomerWishlist> CustomerWishlists { get; set; }


        #endregion

        #region ECommerce
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusCode> OrderStatusCodes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodType> PaymentMethodTypes { get; set; }
        public DbSet<PaymentStatusCode> PaymentStatusCodes { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferProduct> OfferProducts { get; set; }
        public DbSet<OfferCategory> OfferCategorys { get; set; }

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

            // This will apply all configurations in the whole project
            builder.ApplyConfigurationsFromAssembly(typeof(EmployeeConfig).Assembly);

            


        }
    }
}