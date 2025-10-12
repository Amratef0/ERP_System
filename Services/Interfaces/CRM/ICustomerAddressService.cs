using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerAddressService : IGenericService<CustomerAddress>
    {
        Task<IEnumerable<CustomerAddressVM>> GetAllAddressesVMsByCustomerAsync(int customerId);
        Task<CustomerAddressVM> GetAddressByIdAsync(int id);


        Task<bool> AddAddressAsync(CustomerAddressVM model);
        Task<bool> EditAddressAsync(int customerId, CustomerAddressVM model);
        Task<bool> RemoveAddressAsync(int addressId);
        



    }
}
