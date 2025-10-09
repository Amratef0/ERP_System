using ERP_System_Project.Models.Inventory;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System_Project.Services.Interfaces.Inventory
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<ProductAttributeVM>> GetAllProductAttributes();
        Task<PageSourcePagination<ProductVM>> GetProductsPaginated(
            int pageNumber, int pageSize,
            string? searchByName = null,
            string? lowStock = null);
        Task<PageSourcePagination<ProductCardVM>> GetProductsPaginated(
            int pageNumber, int pageSize,
            string? searchByName = null,
            string? brandName = null,
            string? categoryName = null,
            int? minPrice = null, int? maxPrice = null);

        Task<ProductDetailsVM> GetProductDetails(int productId);
        Task AddNewProduct(ProductVM product);
        Task<EditProductVM> GetCustomProduct(int productId);
        Task UpdateCustomProduct(EditProductVM product);

        //Task<PageSourcePagination<AttributeVM>> GetAllAttributesPaginated(int pageNumber, int pageSize, string? searchByName = null);
    }
}
