using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class BrandService : GenericService<Brand>,IBrandService
    {
        private readonly IUnitOfWork _uow;
        public BrandService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public async Task<PageSourcePagination<BrandVM>> GetBrandsPaginated(int pageNumber, int pageSize, string? searchByName = null)
        {
            Expression<Func<Brand, bool>>? searchFilter = null;
            if (!string.IsNullOrEmpty(searchByName))
                searchFilter = p => p.Name.Contains(searchByName);

            return await _uow.Brands.GetAllPaginatedAsync(
                    selector: b => new BrandVM
                    {
                        Description = b.Description,
                        Name = b.Name,
                        Id = b.Id,
                        LogoURL = b.LogoURL,
                        WebsiteURL = b.WebsiteURL ?? "NO Website"
                    },
                    filter: searchFilter,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );
        }
    }
}
