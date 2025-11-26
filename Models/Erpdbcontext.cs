using ERP_System_Project.Models.Config.HRConfig;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.Logs;

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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CustomerFavorite> CustomerFavorites { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<CustomerWishlist> CustomerWishlists { get; set; }


        #endregion

        #region ECommerce
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatusCode> OrderStatusCodes { get; set; }
        public DbSet<PaymentMethodType> PaymentMethodTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<TempCart> TempCarts { get; set; }


        #endregion

        #region Inventory
        public DbSet<Brand> Brands { get; set; }
 public DbSet<Category> Categories { get; set; }
 public DbSet<Product> Products { get; set; }
 public DbSet<ProductAttribute> ProductAttributes { get; set; }
 public DbSet<Warehouse> Warehouses { get; set; }
 public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
 public DbSet<InventoryRequisition> InventoryRequisitions { get; set; }
 public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
 #endregion

        #region HR
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<AttendanceStatusCode> AttendanceStatusCodes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<PayrollEntry> PayrollEntries { get; set; }
        public DbSet<PayrollRun> PayrollRuns { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<WorkScheduleDay> WorkScheduleDays { get; set; }
        #endregion

         #region Purchases
 public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
 public DbSet<Supplier> Suppliers { get; set; }
 public DbSet<SupplierCategory> SupplierCategories { get; set; }
 public DbSet<SupplierProduct> SupplierProducts { get; set; }
 #endregion


        #region Logs
        public DbSet<PerformanceLog> PerformanceLogs { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // PurchaseOrder -> Warehouse
            builder.Entity<PurchaseOrder>()
                .HasOne(p => p.Warehouse)
                .WithMany(w => w.PurchaseOrders)
                .HasForeignKey(p => p.WarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            // PurchaseOrder -> WarehouseProduct
            builder.Entity<PurchaseOrder>()
                .HasOne(p => p.WarehouseProduct)
                .WithMany()
                .HasForeignKey(p => p.WarehouseProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // WarehouseProduct -> Warehouse
            builder.Entity<WarehouseProduct>()
                .HasOne(wp => wp.Warehouse)
                .WithMany(w => w.WarehouseProducts)
                .HasForeignKey(wp => wp.WarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            // SupplierProduct -> Supplier
            builder.Entity<SupplierProduct>()
                .HasOne(sp => sp.Supplier)
                .WithMany()
                .HasForeignKey(sp => sp.SupplierId)
                .OnDelete(DeleteBehavior.NoAction);

            // SupplierProduct -> Product
            builder.Entity<SupplierProduct>()
                .HasOne(sp => sp.Product)
                .WithMany()
                .HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Brand
            builder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product -> Category
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Employee
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.ApplyConfigurationsFromAssembly(typeof(EmployeeConfig).Assembly);
}
    }
}
