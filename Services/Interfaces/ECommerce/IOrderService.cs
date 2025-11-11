namespace ERP_System_Project.Services.Interfaces.ECommerce
{
    public interface IOrderService
    {
        Task MakeOrderAsync(string userId);
    }
}
