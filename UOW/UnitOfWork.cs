using ERP_System_Project.Models;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Repository.Implementation;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Erpdbcontext _db;

        #region Inventory
        public IRepository<Currency> Currencies { get; }
        public IRepository<Country> Countries { get; }
        public IRepository<Address> Addresses { get; }
        public IRepository<Branch> Branches { get; }
        public IRepository<Brand> Brands { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Product> Products { get; }
        public IRepository<ProductAttribute> ProductAttributes { get; }
        public IRepository<ProductVariant> ProductVariants { get; }
        public IRepository<VariantAttributeValue> VariantAttributeValues { get; }
        public IRepository<Warehouse> Warehouses { get; }
        public IRepository<ProductInventory> ProductsInventory { get; }
        public IRepository<InventoryTransactionType> InventoryTransactionTypes { get; }
        public IRepository<InventoryTransaction> InventoryTransactions { get; }
        public IRepository<InventoryRequisitionStatusCode> InventoryRequisitionStatusCodes { get; }
        public IRepository<InventoryRequisition> InventoryRequisitions { get; }
        public IRepository<InventoryRequisitionItem> InventoryRequisitionItems { get; }
        #endregion

        #region ECommerce
        public IRepository<Offer> Offers { get; }
        public IRepository<OfferCategory> OfferCategories { get; }
        public IRepository<OfferProduct> OfferProducts { get; }
        public IRepository<OfferType> OfferTypes { get; }
        public IRepository<Order> Orders { get; }
        public IRepository<OrderItem> OrderItems { get; }
        public IRepository<OrderStatusCode> OrderStatusCodes { get; }
        public IRepository<PaymentMethod> PaymentMethods { get; }
        public IRepository<PaymentMethodType> PaymentMethodTypes { get; }
        public IRepository<PaymentStatusCode> PaymentStatusCodes { get; }
        public IRepository<ShippingMethod> ShippingMethods { get; }
        #endregion

        #region CRM
        public IRepository<Customer> Customers { get; }
        public IRepository<CustomerAddress> CustomerAddresses { get; }
        public IRepository<CustomerFavorite> CustomerFavorites { get; }
        public IRepository<CustomerReview> CustomerReviews { get; }
        public IRepository<CustomerType> CustomerTypes { get; }
        public IRepository<CustomerWishlist> CustomerWishlists { get; }


        #endregion

        #region HR
        public IRepository<AttendanceRecord> AttendanceRecords { get; }
        public IRepository<AttendanceStatusCode> AttendanceStatusCodes { get; }
        public IRepository<Department> Departments { get; }
        public IRepository<Employee> Employees { get; }
        public IRepository<EmployeeLeaveBalance> EmployeeLeaveBalances { get; }
        public IRepository<EmployeeType> EmployeeTypes { get; }
        public IRepository<JobTitle> JobTitles { get; }
        public IRepository<LeaveRequest> LeaveRequests { get; }
        public IRepository<LeaveRequestStatusCode> LeaveRequestStatusCodes { get; }
        public IRepository<LeaveType> LeaveTypes { get; }
        public IRepository<PayrollEntry> PayrollEntries { get; }
        public IRepository<PayrollRun> PayrollRuns { get; }
        public IRepository<PublicHoliday> PublicHolidays { get; }
        public IRepository<WorkScheduleDay> WorkScheduleDays { get; }
        #endregion


        public UnitOfWork(Erpdbcontext db)
        {
            _db = db;

            Currencies = new Repository<Currency>(_db);
            Countries = new Repository<Country>(_db);
            Branches = new Repository<Branch>(_db);
            Addresses = new Repository<Address>(_db);

            Products = new Repository<Product>(_db);
            Brands = new Repository<Brand>(_db);
            Categories = new Repository<Category>(_db);
            ProductAttributes = new Repository<ProductAttribute>(_db);
            ProductVariants = new Repository<ProductVariant>(_db);
            VariantAttributeValues = new Repository<VariantAttributeValue>(_db);
            Warehouses = new Repository<Warehouse>(_db);
            ProductsInventory = new Repository<ProductInventory>(_db);
            InventoryTransactionTypes = new Repository<InventoryTransactionType>(_db);
            InventoryTransactions = new Repository<InventoryTransaction>(_db);
            InventoryRequisitionStatusCodes = new Repository<InventoryRequisitionStatusCode>(_db);
            InventoryRequisitions = new Repository<InventoryRequisition>(_db);
            InventoryRequisitionItems = new Repository<InventoryRequisitionItem>(_db);

            Offers = new Repository<Offer>(_db);
            OfferCategories = new Repository<OfferCategory>(_db);
            OfferProducts = new Repository<OfferProduct>(_db);
            OfferTypes = new Repository<OfferType>(_db);
            Orders = new Repository<Order>(_db);
            OrderItems = new Repository<OrderItem>(_db);
            OrderStatusCodes = new Repository<OrderStatusCode>(_db);
            PaymentMethods = new Repository<PaymentMethod>(_db);
            PaymentMethodTypes = new Repository<PaymentMethodType>(_db);
            PaymentStatusCodes = new Repository<PaymentStatusCode>(_db);
            ShippingMethods = new Repository<ShippingMethod>(_db);


            Customers = new Repository<Customer>(_db);
            CustomerAddresses = new Repository<CustomerAddress>(_db);
            CustomerFavorites = new Repository<CustomerFavorite>(_db);
            CustomerReviews = new Repository<CustomerReview>(_db);
            CustomerTypes = new Repository<CustomerType>(_db);
            CustomerWishlists = new Repository<CustomerWishlist>(_db);

            AttendanceRecords = new Repository<AttendanceRecord>(_db);
            AttendanceStatusCodes = new Repository<AttendanceStatusCode>(_db);
            Departments = new Repository<Department>(_db);
            Employees = new Repository<Employee>(_db);
            EmployeeLeaveBalances = new Repository<EmployeeLeaveBalance>(_db);
            EmployeeTypes = new Repository<EmployeeType>(_db);
            JobTitles = new Repository<JobTitle>(_db);
            LeaveRequests = new Repository<LeaveRequest>(_db);
            LeaveRequestStatusCodes = new Repository<LeaveRequestStatusCode>(_db);
            LeaveTypes = new Repository<LeaveType>(_db);
            PayrollEntries = new Repository<PayrollEntry>(_db);
            PayrollRuns = new Repository<PayrollRun>(_db);
            PublicHolidays = new Repository<PublicHoliday>(_db);
            WorkScheduleDays = new Repository<WorkScheduleDay>(_db);
        }

        public async Task<int> CompleteAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_db);
        }
    }
}
