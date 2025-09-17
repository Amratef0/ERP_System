using ERP_System_Project.Models.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerService
    {
        Task<bool> SoftDeleteCustomerAsync(int id);    
        Task<bool> ReactivateCustomerAsync(int id);
        Task<Customer?> GetByIdAsync(int id, bool includeInactive = false);
    }
}
