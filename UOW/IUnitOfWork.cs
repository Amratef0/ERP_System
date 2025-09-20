using ERP_System_Project.Models;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Repository.Interfaces;

namespace ERP_System_Project.UOW
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        IRepository<Currency> Currencies { get; }
        IRepository<Country> Countries { get; }
        IRepository<Address> Addresses { get; }
        IRepository<Branch> Branches { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductAttribute> ProductAttributes { get; }
        IRepository<ProductType> ProductTypes { get; }
        IRepository<ProductVariant> ProductVariants { get; }
        IRepository<UnitOfMeasure> UnitsOfMeasure { get; }
        IRepository<VariantAttributeValue> VariantAttributeValues { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<ProductInventory> ProductsInventory { get; }
        IRepository<InventoryTransactionType> InventoryTransactionTypes { get; }
        IRepository<InventoryTransaction> InventoryTransactions { get; }
        IRepository<InventoryRequisitionStatusCode> InventoryRequisitionStatusCodes { get; }
        IRepository<InventoryRequisition> InventoryRequisitions { get; }
        IRepository<InventoryRequisitionItem> InventoryRequisitionItems { get; }

        IRepository<Offer> Offers { get; }
        IRepository<OfferCategory> OfferCategories { get; }
        IRepository<OfferProduct> OfferProducts { get; }
        IRepository<OfferType> OfferTypes { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<OrderStatusCode> OrderStatusCodes { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        IRepository<PaymentMethodType> PaymentMethodTypes { get; }
        IRepository<PaymentStatusCode> PaymentStatusCodes { get; }
        IRepository<ShippingMethod> ShippingMethods { get; }

        IRepository<Customer> Customers { get; }
        IRepository<CustomerAddress> CustomerAddresses { get; }
        IRepository<CustomerFavorite> CustomerFavorites { get; }
        IRepository<CustomerReview> CustomerReviews { get; }
        IRepository<CustomerType> CustomerTypes { get; }
        IRepository<CustomerWishlist> CustomerWishlists { get; }

        Task<int> CompleteAsync();

    }
}
