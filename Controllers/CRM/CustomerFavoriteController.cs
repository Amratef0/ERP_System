using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Implementation.Inventory;
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
    public class CustomerFavoriteController : Controller
    {
        private readonly ICustomerFavoriteService _customerFavoriteService;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerFavoriteController(ICustomerFavoriteService customerFavoriteService, IProductService productService, IBrandService brandService,ICategoryService categoryService,UserManager<ApplicationUser> userManager)
        {
            _customerFavoriteService = customerFavoriteService;
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

            var favorites = await _productService.GetFavoriteProductsPaginatedAsync(
                customerId, pageNumber, pageSize, searchByName, brandName, categoryName, minPrice, maxPrice);

            ViewBag.CustomerId = customerId;
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int customerId, int productId)
        {
            try
            {
                var result = await _customerFavoriteService.ToggleFavoriteAsync(customerId, productId);

                // we will do nothing and stay at the same page but return to debugging
                return Json(new
                {
                    success = true,
                    status = result ? "added" : "removed",
                    message = result ? "Added to favorites!" : "Removed from favorites!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating favorites."
                });
            }



        }

        // we can add (addFavorite and remove favorite ) instead of toggle favorite if needed (already implemented in the service)

        [HttpGet]
        public async Task<JsonResult> CheckFavoriteStatus(int customerId, int productId)
        {
            var isFavorite = await _customerFavoriteService.IsFavoriteAsync(customerId, productId);
            return Json(new { isFavorite });
        }



    }
}
