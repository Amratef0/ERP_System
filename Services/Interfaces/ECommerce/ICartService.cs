using ERP_System_Project.ViewModels.ECommerce;

namespace ERP_System_Project.Services.Interfaces.ECommerce
{
    public interface ICartService
    {
        Task<CartVM> GetAllFromCart();
        void AddToCart(int productId, int quantity);
        void RemoveFromCart(int productId);
        Task ClearCartAsync();

    }
}
