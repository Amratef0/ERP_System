using ERP_System_Project.Models.Core;

namespace ERP_System_Project.Services.Interfaces.Core
{
    public interface IBranchService : IGenericService<Branch>
    {
        Task<Branch> GetWithAddressAsync(int id);
        Task<IEnumerable<Branch>> FilterAsync(string name, int countryId);
    }
}