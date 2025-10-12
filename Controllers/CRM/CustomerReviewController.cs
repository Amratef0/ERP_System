using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Composition.Convention;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerReviewController : Controller
    {
        private readonly ICustomerReviewService _customerReviewService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IProductService _productService;

        public CustomerReviewController(ICustomerReviewService customerReviewService, UserManager<ApplicationUser> userManager
            , IUnitOfWork uow,
            IProductService productService)
        {

            _customerReviewService = customerReviewService;
            _userManager = userManager;
            _uow = uow;
            _productService = productService;
        }
        [Authorize]
        private async Task<int> GetLoggedInUserCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            var customerId = user?.CustomerId ?? 0;
            Console.WriteLine(customerId);
            return customerId;

        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllReviewsForCustomer()
        {
            int customerId = await GetLoggedInUserCustomerId();
            if (customerId == 0) return Unauthorized();
            var reviews = await _customerReviewService.GetAllReviewsVMsForCustomerAsync(customerId);

            return View(reviews);
        }
        public async Task<IActionResult> GetAllReviewsForProduct(int productId)
        {
            var reviews = await _customerReviewService.GetAllReviewsVMsForProductAsync(productId);

            // i need to get product info from the product page iam in how to?

            var averageRating = await _customerReviewService.GetAverageRatingAsync(productId);
            var reviewCount = await _customerReviewService.GetReviewCountAsync(productId);
            var product = await _productService.GetProductDetails(productId);
            ViewBag.ProductDetails = product;
            ViewBag.AverageRating = averageRating;
            ViewBag.ReviewCount = reviewCount;
            return View(reviews);

        }

        //public IActionResult AddReview()
        //{

        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview(CustomerReviewVM model)
        {

            if (ModelState.IsValid)
            {
                var customerId = await GetLoggedInUserCustomerId();
                if (customerId == 0) return Unauthorized();
                model.CustomerId = customerId;
                var added = await _customerReviewService.AddReviewAsync(model);
                if (!added)
                {
                    ModelState.AddModelError(string.Empty, "Unable to add review. You may have already reviewed this product.");
                    return View(model);
                }
                return RedirectToAction("Index"); // we will edit this to return to the product page to see his review after adding

            }
            return View(model);

        }
        public async Task<IActionResult> EditReview(int reviewId)
        {
            if (reviewId <= 0) return BadRequest();
            var review = await _customerReviewService.GetByIdAsync(reviewId);
            if (review == null) return NotFound();
            var model = new CustomerReviewVM
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating,
                CustomerId = review.CustomerId,
                ProductId = review.ProductId,
                CreatedAt = review.CreatedAt,
                IsEdited = review.IsEdited,
                EditedAt = review.EditedAt
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(CustomerReviewVM model)
        {
            if (model == null) return BadRequest();
            if (model.Id <= 0) return BadRequest();


            if (ModelState.IsValid)
            {
                var updated = await _customerReviewService.UpdateReviewAsync(model.Id, model);
                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "Unable to update review. Please try again.");
                    return View(model);
                }
                return RedirectToAction("Index"); // we will edit this to return to the product page to see his review after adding
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _customerReviewService.GetByIdAsync(reviewId);
            if (review == null) return BadRequest();

            var deleted = await _customerReviewService.DeleteAsync(reviewId);
            if (!deleted) return BadRequest();
            return RedirectToAction("Index"); // we will edit this to return to the product page to see his review after adding
        }
    }
}
