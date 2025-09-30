using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.Core
{
    public class BranchService : GenericService<Branch>, IBranchService
    {
        public BranchService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<Branch> GetWithAddressAsync(int id)
        {
            var query = _repository.GetAllAsIQueryable();

            var branch = query.Include(b => b.Address)
                              .ThenInclude(b => b.Country)
                              .FirstOrDefault(b => b.Id == id);

            return branch;
        }

        public async Task<IEnumerable<Branch>> FilterAsync(string name, int countryId)
        {
            var query = _repository.GetAllAsIQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.ToUpper().StartsWith(name.ToUpper()));
            }

            if (countryId > 0)
            {
                query = query.Where(b => b.Address.CountryId == countryId);
            }

            return await query.ToListAsync();
        }
    }
}
