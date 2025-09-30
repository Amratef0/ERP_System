using ERP_System_Project.Models.Core;

namespace ERP_System_Project.Services.Interfaces.Core
{
    public interface ICurrencyService : IGenericService<Currency>
    {
        Task<IEnumerable<Currency>> SearchByNameAsync(string name);
    }
}
