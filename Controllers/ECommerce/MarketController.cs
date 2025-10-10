using ERP_System_Project.Services.Implementation.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_System_Project.Controllers.ECommerce
{
    public class MarketController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public MarketController(IProductService productService, IBrandService brandService, ICategoryService categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
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
            int pageNumber, int pageSize,
            string? searchByName = null, string? brandName = null, string? categoryName = null,
            int? minPrice = null, int? maxPrice = null)
        {
            await CreateBrandOptions();
            await CreateCategoryOptions();

            var products = await _productService.GetProductsPaginated(
                pageNumber, pageSize,searchByName,brandName,categoryName,
                minPrice, maxPrice
                );
            
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(int productId)
        {
            var product = await _productService.GetProductDetails(productId);
            if (product != null) return View(product);
            return NotFound();
        }
    }
}
