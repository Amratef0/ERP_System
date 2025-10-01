using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class JobTitleService : GenericService<JobTitle>, IJobTitleService
    {

        public JobTitleService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<IEnumerable<JobTitle>> SearchAsync(string name)
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
