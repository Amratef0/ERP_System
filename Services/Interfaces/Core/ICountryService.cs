using ERP_System_Project.Models.Core;

namespace ERP_System_Project.Services.Interfaces.Core
{
    public interface ICountryService : IGenericService<Country>
    {
        Task<List<Country>> GetAllAsync();
    }
}
