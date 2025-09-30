using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class PublicHolidayService : GenericService<PublicHoliday>, IPublicHolidayService
    {
        public PublicHolidayService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<IEnumerable<PublicHoliday>> GetAllWithCountryAsync()
        {
            var publicHolidays = await _repository.GetAllAsync(selector: ph => ph, Includes: ph => ph.Country);
            return publicHolidays;
        }

        public async Task<IEnumerable<PublicHoliday>> FilterAsync(string name, int countryId)
        {
            var query = _repository.GetAllAsIQueryable();

            query = query.Include(ph => ph.Country);

            if (countryId > 0)
            {
                query = query.Where(ph => ph.CountryId == countryId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(ph => ph.Name.ToUpper().StartsWith(name.ToUpper()));
            }

            return await query.ToListAsync();
        }
    }
}