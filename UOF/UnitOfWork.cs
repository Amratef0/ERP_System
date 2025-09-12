using ERP_System_Project.Models;
using ERP_System_Project.Repository.Implementations;
using ERP_System_Project.Repository.Interfaces;

namespace ERP_System_Project.UOF
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


        }

        public async Task<int> CompleteAsync() => await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
