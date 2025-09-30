using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class EmployeeTypeService : GenericService<EmployeeType>, IEmployeeTypeService
    {
        public EmployeeTypeService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<IEnumerable<EmployeeType>> SearchAsync(string name)
        {
            var query = _repository.GetAllAsIQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(et => et.Name.ToUpper().StartsWith(name.ToUpper()));
            }

            return await query.ToListAsync();
        }
    }
}
