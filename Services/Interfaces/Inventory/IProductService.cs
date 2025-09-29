using ERP_System_Project.Models.Inventory;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System_Project.Services.Interfaces.Inventory
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<ProductAttributeVM>> GetAllProductAttributes();
        Task<PageSourcePagination<ProductVM>> GetProductsPaginated(int pageNumber, int pageSize, string? searchByName = null);
        Task AddNewProduct(ProductVM product);
        Task<EditProductVM> GetCustomProduct(int productId);
        Task UpdateCustomProduct(EditProductVM product);
    }
}
