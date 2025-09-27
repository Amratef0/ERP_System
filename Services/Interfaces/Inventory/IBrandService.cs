using ERP_System_Project.Models.Inventory;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;

namespace ERP_System_Project.Services.Interfaces.Inventory
{
    public interface IBrandService : IGenericService<Brand>
    {
        Task<PageSourcePagination<BrandVM>> GetBrandsPaginated(int pageNumber, int pageSize, string searchByName);
    }
}
