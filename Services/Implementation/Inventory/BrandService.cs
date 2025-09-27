using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class BrandService : GenericService<Brand>,IBrandService
    {
        private readonly IUnitOfWork _uow;
        public BrandService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public Task<PageSourcePagination<BrandVM>> GetBrandsPaginated(int pageNumber, int pageSize, string? searchByName = null)
        {
            if (!string.IsNullOrEmpty(searchByName))
            {
                return _uow.Brands.GetAllPaginatedAsync(
                    selector: b => new BrandVM
                    {
                        Description = b.Description,
                        Name = b.Name,
                        Id = b.Id,
                        LogoURL = b.LogoURL,
                        WebsiteURL = b.WebsiteURL
                    },
                    filter: b => b.Name.Contains(searchByName),
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );
            }

            return _uow.Brands.GetAllPaginatedAsync(
                selector: b => new BrandVM
                {
                    Description = b.Description,
                    Name = b.Name,
                    Id = b.Id,
                    LogoURL = b.LogoURL,
                    WebsiteURL = b.WebsiteURL
                },
                pageNumber: pageNumber,
                pageSize: pageSize
            );
        }

    }
}
