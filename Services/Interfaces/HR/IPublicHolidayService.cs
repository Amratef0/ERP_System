using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IPublicHolidayService : IGenericService<PublicHoliday>
    {
        Task<IEnumerable<PublicHoliday>> GetAllWithCountryAsync();
        Task<IEnumerable<PublicHoliday>> FilterAsync(string name, int countryId);
    }
}
