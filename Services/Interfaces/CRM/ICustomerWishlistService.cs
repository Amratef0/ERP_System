using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerWishlistService : IGenericService<CustomerWishlist>
    {
        Task<IEnumerable<CustomerWishlistVM>> GetAllWishlistsVMsAsync(int? customerId = null);

        Task<bool> ToggleWishlistAsync(int customerId, int productId);
        Task<bool> RemoveWishlistAsync(int customerId, int productId);
        Task<bool> AddWishlistAsync(int customerId, int productId);
        Task<bool> IsWishlistAsync(int customerId, int productId);
        



    }
}
