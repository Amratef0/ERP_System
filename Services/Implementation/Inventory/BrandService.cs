using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Services.Implementation.Inventory
{
    public class BrandService : GenericService<Brand>,IBrandService
    {
        private readonly IUnitOfWork _uow;
        public BrandService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }
    }
}
