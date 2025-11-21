using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.Enums;

namespace ERP_System_Project.Services.Interfaces.ECommerce
{
    public interface IOrderService
    {
        // عدلنا الدالة هنا عشان تاخد CartViewModel
        Task MakeOrderAsync(string userId, CartViewModel cart, PaymentMethod paymentMethod);

        Task<PageSourcePagination<MyOrdersVM>> GetCustomerOrdersAsync(string userId, int pageNumber, int pageSize);

        Task<CartVM> GetCartVMAsync(string userId);

        Task SaveCartForPayment(string userName, CartViewModel cart);
        Task<CartViewModel> GetCartFromPayment(string userName);
    }

}
