using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;

namespace ERP_System_Project.Services.Interfaces.ECommerce
{
    public interface IOrderService
    {
        Task MakeOrderAsync(string userId);
        Task<PageSourcePagination<MyOrdersVM>> GetCustomerOrdersAsync(string userId, int pageNumber, int pageSize);
    }
}
