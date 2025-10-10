using ERP_System_Project.ViewModels.ECommerce;

namespace ERP_System_Project.Services.Interfaces.ECommerce
{
    public interface IOfferService
    {
        Task<OfferVM> GetOfferAsync(int productId);
        Task SetOfferAsync(OfferVM model);
    }
}
