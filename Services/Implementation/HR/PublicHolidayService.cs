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

        public new async Task<List<PublicHoliday>> GetAllAsync()
        {
            var PublicHolidays = await base.GetAllAsync();
            return PublicHolidays.ToList();
        }
    }
}