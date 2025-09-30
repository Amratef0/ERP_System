using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class CategoryService : GenericService<Category>,ICategoryService
    {
        private readonly IUnitOfWork _uow;
        public CategoryService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public Task<PageSourcePagination<CategoryVM>> GetCategoriesPaginated(int pageNumber, int pageSize, string? searchByName = null)
        {
            Expression<Func<Category, bool>>? searchFilter = null;
            if (!string.IsNullOrEmpty(searchByName))
                searchFilter = p => p.Name.Contains(searchByName);

            return _uow.Categories.GetAllPaginatedAsync(
                    selector: c => new CategoryVM
                    {
                        Description = c.Description,
                        Name = c.Name,
                        Id = c.Id,
                       
                    },
                    filter: searchFilter,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );
        }
    }
}
