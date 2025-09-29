using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ERP_System_Project.Controllers.Inventory
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        public ProductController(
            IProductService productService, ICategoryService categoryService, IBrandService brandService
            ,IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _mapper = mapper;
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
        private async Task CreateAttributeOptions(int id = 1)
        {
            var attributes = await _productService.GetAllProductAttributes();
            SelectList selectAttributeList = new SelectList(attributes, "Id", "Name", id);
            ViewBag.Attributes = selectAttributeList;
        }

        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var products = await _productService.GetProductsPaginated(pageNumber,pageSize,searchByName);
            return View(products);
        }

        public async Task<IActionResult> New()
        {
            await CreateBrandOptions();
            await CreateCategoryOptions();
            await CreateAttributeOptions();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(ProductVM productVM)
        {
            await CreateBrandOptions();
            await CreateCategoryOptions();
            await CreateAttributeOptions();
            if (productVM.BrandId == 0)
                ModelState.AddModelError("BrandId", "Please select a brand");

            if (productVM.CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Please select a category");

            for (int i = 0; i < productVM.ProductVariants.Count; i++)
            {
                if (productVM.ProductVariants[i].AtrributeId == 0)
                {
                    ModelState.AddModelError($"ProductVariants[{i}].AtrributeId", "Attribute is required");
                }
            }
            if (ModelState.IsValid)
            {
                await _productService.AddNewProduct(productVM);
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();
            var productVM = _mapper.Map<ProductVM>(product);

            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var product = await _productService.GetByIdAsync(productVM.Id);
                _mapper.Map(productVM, product);
                await _productService.UpdateAsync(product);
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();
            var productVM = _mapper.Map<ProductVM>(product);
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductVM productVM)
        {
            await _productService.DeleteAsync(productVM.Id);
            return RedirectToAction("Index");
        }
    }
}
