using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.Core
{
    public class CurrencyService : GenericService<Currency>, ICurrencyService
    {
        public CurrencyService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<IEnumerable<Currency>> SearchByNameAsync(string name)
        {
            var query = _repository.GetAllAsIQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.ToUpper().StartsWith(name.ToUpper()));
            }

            return await query.ToListAsync();
        }
    }
}
