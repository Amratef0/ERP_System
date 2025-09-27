using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.Inventory
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        public BrandController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var brands = await _brandService.GetBrandsPaginated(pageNumber,pageSize,searchByName);
            return View(brands);
        }

        public async Task<IActionResult> New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(BrandVM brandVM)
        {
            if (ModelState.IsValid)
            {
                var brand = _mapper.Map<Brand>(brandVM);
                await _brandService.CreateAsync(brand);
                return RedirectToAction("Index");
            }
            return View(brandVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand is null) return NotFound();
            var brandVM = _mapper.Map<BrandVM>(brand);

            return View(brandVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandVM brandVM)
        {
            if (ModelState.IsValid)
            {
                var brand = await _brandService.GetByIdAsync(brandVM.Id);
                // update using mapper
                _mapper.Map(brandVM, brand); 
                await _brandService.UpdateAsync(brand);
                return RedirectToAction("Index");
            }
            return View(brandVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand is null) return NotFound();
            var brandVM = _mapper.Map<BrandVM>(brand);
            return View(brandVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(BrandVM brandvm)
        {
            await _brandService.DeleteAsync(brandvm.Id);
            return RedirectToAction("Index");
        }
    }
}
