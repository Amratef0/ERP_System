using ERP_System_Project.Models;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Repository.Interfaces;

namespace ERP_System_Project.UOW
{
    public interface IUnitOfWork
    {
        IRepository<Currency> Currencies { get; }
        IRepository<Country> Countries { get; }
        IRepository<Address> Addresses { get; }
        IRepository<Branch> Branches { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductAttribute> ProductAttributes { get; }
        IRepository<ProductType> ProductTypes { get; }
        IRepository<ProductVariant> ProductVariants { get; }
        IRepository<UnitOfMeasure> UnitsOfMeasure { get; }
        IRepository<VariantAttributeValue> VariantAttributeValues { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<ProductInventory> ProductsInventory { get; }
        IRepository<InventoryTransactionType> InventoryTransactionTypes { get; }

        Task<int> CompleteAsync();

    }
}
