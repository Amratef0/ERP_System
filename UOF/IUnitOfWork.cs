using ERP_System_Project.Models;
using ERP_System_Project.Repository.Interfaces;

namespace ERP_System_Project.UOF
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

        Task<int> CompleteAsync();

    }
}
