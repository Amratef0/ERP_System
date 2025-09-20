using ERP_System_Project.Models.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerService : IGenericService<Customer>
    {
        Task<bool> SoftDeleteCustomerAsync(int id);    
        Task<bool> ReactivateCustomerAsync(int id);
        Task<Customer?> GetCustomerByIdAsync(int id, bool includeInactive = false);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(bool includeInactive = false);

    }
}
