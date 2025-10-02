using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerFavoriteService : IGenericService<CustomerFavorite>
    {
        Task<IEnumerable<CustomerFavoriteVM>> GetAllFavoritesVMsAsync(int? customerId = null);

        Task<bool> ToggleFavoriteAsync(int customerId, int productId);
        Task<bool> RemoveFavoriteAsync(int customerId, int productId);
        Task<bool> AddFavoriteAsync(int customerId, int productId);
        Task<bool> IsFavoriteAsync(int customerId, int productId);
        



    }
}
