using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerService : IGenericService<Customer>
    {
        Task<bool> SoftDeleteCustomerAsync(int id);    
        Task<bool> ReactivateCustomerAsync(int id);
        Task<Customer?> GetCustomerByIdAsync(int id, bool includeInactive = false);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(bool includeInactive = false);
        Task CreateCustomerByApplicationUserAsync(ApplicationUser user, RegisterViewModel model);



        Task<PageSourcePagination<CustomerVM>> GetCustomersPaginatedAsync(
                   int pageNumber,
                   int pageSize,
                   string? searchByName = null,
                   bool includeInactive = false);
        Task<CustomerVM?> GetCustomerVMByIdAsync(int id);
        Task<bool> CreateCustomerVMAsync(CustomerVM customerVM);
        Task<bool> UpdateCustomerVMAsync(CustomerVM customerVM);

    }
}
