using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.Inventory;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class AttributeService : GenericService<ProductAttribute>, IAttributeService
    {
        private readonly IUnitOfWork _uow;
        public AttributeService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public async Task<PageSourcePagination<AttributeVM>> GetAllAttributesPaginated(int pageNumber, int pageSize, string? searchByName = null)
        {
            Expression<Func<ProductAttribute, bool>>? searchFilter = null;

            if (!string.IsNullOrEmpty(searchByName))
                searchFilter = p => p.Name.Contains(searchByName);

            return await _uow.ProductAttributes.GetAllPaginatedAsync(
                selector: pa => new AttributeVM
                {
                    Id = pa.Id,
                    Name = pa.Name,
                    Type = pa.Type,
                },
                filter: searchFilter,
                pageNumber: pageNumber,
                pageSize: pageSize
            );
        }
    }
}
