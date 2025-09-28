using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Services.Implementation.HR
{
    public class PublicHolidayService : GenericService<PublicHoliday>, IPublicHolidayService
    {
        public PublicHolidayService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<List<PublicHoliday>> SearchByNameAsync(string name)
        {
            var filteredPublicHolidays = await _repository.GetAllAsync(selector: ph => ph,
                                                    filter: ph => ph.Name.ToUpper().Contains(name.ToUpper()));
            return filteredPublicHolidays.ToList();
        }
    }
}