using AutoMapper;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerWishlistController : Controller
    {
        private readonly ICustomerWishlistService _customerWishlistService;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerWishlistController(ICustomerWishlistService customerWishlistService, IProductService productService, IBrandService brandService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _customerWishlistService = customerWishlistService;
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        private async Task<int> GetLoggedInUserCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.CustomerId ?? 0;
        }
        private async Task CreateBrandOptions(int id = 1)
        {
            var brands = await _brandService.GetAllAsync();
            SelectList selectBrandList = new SelectList(brands, "Id", "Name", id);
            ViewBag.Brands = selectBrandList;
        }

        private async Task CreateCategoryOptions(int id = 1)
        {
            var categories = await _categoryService.GetAllAsync();
            SelectList selectCategoryList = new SelectList(categories, "Id", "Name", id);
            ViewBag.Categories = selectCategoryList;
        }
        public async Task<IActionResult> Index(
             int pageNumber = 1,
             int pageSize = 12,
             string? searchByName = null,
             string? brandName = null,
             string? categoryName = null,
             int? minPrice = null,
             int? maxPrice = null)
        {
            var customerId = await GetLoggedInUserCustomerId();

            if (customerId == 0)
                return RedirectToAction("Login", "Account");

            await CreateBrandOptions();
            await CreateCategoryOptions();

            var wishlists = await _productService.GetWishlistProductsPaginatedAsync(
                customerId, pageNumber, pageSize, searchByName, brandName, categoryName, minPrice, maxPrice);

            ViewBag.CustomerId = customerId;
            return View(wishlists);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleWishlist(int customerId, int productId)
        {
            try
            {
                var result = await _customerWishlistService.ToggleWishlistAsync(customerId, productId);

                // we will do nothing and stay at the same page but return to debugging
                return Json(new
                {
                    success = true,
                    status = result ? "added" : "removed",
                    message = result ? "Added to wishlists!" : "Removed from wishlists!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating wishlists."
                });
            }



        }

        // we can add (add Wishlist and remove Wishlist ) instead of toggle Wishlist if needed (already implemented in the service)

        [HttpGet]
        public async Task<JsonResult> CheckWishlistStatus(int customerId, int productId)
        {
            var isWishlist = await _customerWishlistService.IsWishlistAsync(customerId, productId);
            return Json(new { isWishlist });
        }



    }
}
