using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IPublicHolidayService : IGenericService<PublicHoliday>
    {
        Task<List<PublicHoliday>> SearchByNameAsync(string name);
    }
}
