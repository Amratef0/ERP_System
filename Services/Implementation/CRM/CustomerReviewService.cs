using ERP_System_Project.Models.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerReviewService : GenericService<CustomerReview>, ICustomerReviewService
    {
        private readonly IUnitOfWork _uow;
        public CustomerReviewService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }
        public async Task<bool> HasCustomerReviewedProductAsync(int customerId, int productId)
        {
            var reviews= _uow.CustomerReviews.GetAllAsIQueryable();
              return await reviews.AnyAsync(cr => cr.ProductId == productId && cr.CustomerId == customerId);
        }
        public async Task<bool> AddReviewAsync(CustomerReviewVM model)
        {
            try {

                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Review model cannot be null");

                // Check if the customer has already reviewed the product
                bool hasReviewed = await HasCustomerReviewedProductAsync(model.CustomerId, model.ProductId);
                if (hasReviewed)
                    throw new InvalidOperationException("Customer has already reviewed this product.");



                var customer = await _uow.Customers.GetByIdAsync(model.CustomerId);
                var product = await _uow.Products.GetByIdAsync(model.ProductId);

                if (customer == null)
                    throw new ArgumentException("Customer not found");
                if (product == null)
                    throw new ArgumentException("Product not found");

                var review = new CustomerReview
                {
                    CustomerId = model.CustomerId,
                    ProductId = model.ProductId,
                    Rating = model.Rating,
                    Comment = model.Comment,
                    CreatedAt = DateTime.Now
                };
                await _uow.CustomerReviews.AddAsync(review);
                await _uow.CompleteAsync();
                return true;


            }
            catch(Exception ex)
            {
            return false;
            }


        }


        public async Task<IEnumerable<CustomerReviewVM>> GetAllReviewsVMsForCustomerAsync(int customerId)
        {
            var reviews = await _uow.CustomerReviews.GetAllAsIQueryable()
                .Include(cr=>cr.Customer)
                .Include(cr=>cr.Product)
                .Where(cr=>cr.CustomerId == customerId)
                .OrderByDescending(cr=>cr.CreatedAt)
                .Select(cr=> new CustomerReviewVM
                {
                    Id= cr.Id,
                    CustomerId= cr.CustomerId,
                    ProductId= cr.ProductId,
                    ProductName= cr.Product.Name,
                    CustomerName= $"{cr.Customer.FirstName} {cr.Customer.LastName}",
                    Rating= cr.Rating,
                    Comment= cr.Comment,
                    CreatedAt= cr.CreatedAt,
                    IsEdited= cr.IsEdited,
                    EditedAt = cr.EditedAt
                    
                }).ToListAsync();
            return reviews;
        }

        public async Task<IEnumerable<CustomerReviewVM>> GetAllReviewsVMsForProductAsync(int productId)
        {
            var reviews = await _uow.CustomerReviews.GetAllAsIQueryable()
                .Include(cr=>cr.Customer)
                .Include(cr=>cr.Product)
                .Where(cr=>cr.ProductId == productId)
                .OrderByDescending(cr => cr.CreatedAt)
                .Select(cr=> new CustomerReviewVM
                {
                    Id = cr.Id,
                    CustomerId = cr.CustomerId,
                    ProductId = cr.ProductId,
                    ProductName = cr.Product.Name,
                    CustomerName = $"{cr.Customer.FirstName} {cr.Customer.LastName}",
                    Rating = cr.Rating,
                    Comment = cr.Comment,
                    CreatedAt = cr.CreatedAt,
                    IsEdited = cr.IsEdited,
                    EditedAt = cr.EditedAt
                }).ToListAsync();

            return reviews;
        }

        public async Task<decimal> GetAverageRatingAsync(int productId)
        {
            var avg=  await _uow.CustomerReviews.GetAllAsIQueryable()
                .Where(cr=>cr.ProductId == productId)
                .AverageAsync(cr=>(double?)cr.Rating);// double to handle bull (chat gpt)
            return (decimal)(avg ?? 0);
        }

        public async Task<int> GetReviewCountAsync(int productId)
        {
            return await _uow.CustomerReviews.GetAllAsIQueryable()
                .Where(cr=>cr.ProductId == productId)
                .CountAsync();
        }
        
        

        public async Task<bool> UpdateReviewAsync(int reviewId, CustomerReviewVM model)
        {
            try
            {
                var review = await _uow.CustomerReviews.GetByIdAsync(reviewId);
                review.Rating = model.Rating;

                review.Comment = model.Comment;
                review.IsEdited = true;
                review.EditedAt = DateTime.Now;
                _uow.CustomerReviews.Update(review);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override async Task<CustomerReview?> GetByIdAsync(int id)
        {
            return await _uow.CustomerReviews.GetAllAsIQueryable()
                .Include(cr=>cr.Customer)
                .Include(cr=>cr.Product)
                .FirstOrDefaultAsync(cr=>cr.Id == id);
        }
    }
}
