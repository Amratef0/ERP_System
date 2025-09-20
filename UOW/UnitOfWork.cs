using ERP_System_Project.Models;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.CRM;
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
        public IRepository<Currency> Currencies { get; }
        public IRepository<Country> Countries { get; }
        public IRepository<Address> Addresses { get; }
        public IRepository<Branch> Branches { get; }
        public IRepository<Brand> Brands { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Order> Orders { get; }
        public IRepository<Product> Products { get; }
        public IRepository<ProductAttribute> ProductAttributes { get; }
        public IRepository<ProductType> ProductTypes { get; }
        public IRepository<ProductVariant> ProductVariants { get; }
        public IRepository<UnitOfMeasure> UnitsOfMeasure { get; }
        public IRepository<VariantAttributeValue> VariantAttributeValues { get; }
        public IRepository<Warehouse> Warehouses { get; }
        public IRepository<ProductInventory> ProductsInventory { get; }
        public IRepository<InventoryTransactionType> InventoryTransactionTypes { get; }
        public IRepository<InventoryTransaction> InventoryTransactions { get; }
        public IRepository<InventoryRequisitionStatusCode> InventoryRequisitionStatusCodes { get; }
        public IRepository<InventoryRequisition> InventoryRequisitions { get; }
        public IRepository<InventoryRequisitionItem> InventoryRequisitionItems { get; }

        #region
        public IRepository<Customer> Customers { get;}
        public IRepository<CustomerAddress> CustomerAddresses { get; }
        public IRepository<CustomerFavorite> CustomerFavorites { get; }
        public IRepository<CustomerReview> CustomerReviews { get; }
        public IRepository<CustomerType> CustomerTypes { get; }
        public IRepository<CustomerWishlist> CustomerWishlists { get; }


        #endregion


        public UnitOfWork(Erpdbcontext db)
        {
            _db = db;
            Currencies = new Repository<Currency>(_db);
            Countries = new Repository<Country>(_db);
            Branches = new Repository<Branch>(_db);
            Products = new Repository<Product>(_db);
            Addresses = new Repository<Address>(_db);
            Brands = new Repository<Brand>(_db);
            Categories = new Repository<Category>(_db);
            Orders = new Repository<Order>(_db);
            ProductAttributes = new Repository<ProductAttribute>(_db);
            ProductTypes = new Repository<ProductType>(_db);
            ProductVariants = new Repository<ProductVariant>(_db);
            UnitsOfMeasure =new Repository<UnitOfMeasure>(_db);
            VariantAttributeValues = new Repository<VariantAttributeValue>(_db);
            Warehouses = new Repository<Warehouse>(_db);
            ProductsInventory = new Repository<ProductInventory>(_db);
            InventoryTransactionTypes = new Repository<InventoryTransactionType>(_db);
            InventoryTransactions = new Repository<InventoryTransaction>(_db);
            InventoryRequisitionStatusCodes = new Repository<InventoryRequisitionStatusCode>(_db);
            InventoryRequisitions = new Repository<InventoryRequisition>(_db);
            InventoryRequisitionItems = new Repository<InventoryRequisitionItem>(_db);

        Customers = new Repository<Customer>(_db);
            CustomerAddresses = new Repository<CustomerAddress>(_db);
            CustomerFavorites = new Repository<CustomerFavorite>(_db);
            CustomerReviews = new Repository<CustomerReview>(_db);
            CustomerTypes = new Repository<CustomerType>(_db);
            CustomerWishlists = new Repository<CustomerWishlist>(_db);


        }

        public async Task<int> CompleteAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_db);
        }
    }
}
