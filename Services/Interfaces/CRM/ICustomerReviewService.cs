using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Services.Interfaces.CRM
{
    public interface ICustomerReviewService : IGenericService<CustomerReview>
    {
        Task <IEnumerable<CustomerReviewVM>> GetAllReviewsVMsForProductAsync(int productId );
        Task <IEnumerable<CustomerReviewVM>> GetAllReviewsVMsForCustomerAsync(int customerId );
        Task <bool> AddReviewAsync(CustomerReviewVM model);
        Task <bool> UpdateReviewAsync(int reviewId, CustomerReviewVM model);
        Task<bool> HasCustomerReviewedProductAsync(int customerId, int productId);
        Task<int> GetReviewCountAsync(int productId);
        Task<decimal> GetAverageRatingAsync(int productId);

    }
}
