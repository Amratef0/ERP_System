using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ERP_System_Project.Controllers.Inventory
{
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IValidator<BrandVM> _validator;
        private readonly IMapper _mapper;
        public BrandController(IBrandService brandService, IMapper mapper, IValidator<BrandVM> validator)
        {
            _brandService = brandService;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var brands = await _brandService.GetBrandsPaginated(pageNumber,pageSize,searchByName);
            return View(brands);
        }

        public async Task<IActionResult> New()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(BrandVM brandVM)
        {
            var result = _validator.Validate(brandVM);
            if (result.IsValid)
            {
                var brand = _mapper.Map<Brand>(brandVM);
                await _brandService.CreateAsync(brand);
                TempData["SuccessMessage"] = $"Brand {brand.Name} Created Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(brandVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand is null) return NotFound();
            var brandVM = _mapper.Map<BrandVM>(brand);

            return PartialView(brandVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandVM brandVM)
        {
            var result = _validator.Validate(brandVM);

            if (result.IsValid)
            {
                var brand = await _brandService.GetByIdAsync(brandVM.Id);
                // update using mapper
                _mapper.Map(brandVM, brand); 
                await _brandService.UpdateAsync(brand);
                TempData["SuccessMessage"] = $"Brand {brand.Name} Updated Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(brandVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand is null) return NotFound();
            var brandVM = _mapper.Map<BrandVM>(brand);
            return PartialView(brandVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(BrandVM brandvm)
        {
            await _brandService.DeleteAsync(brandvm.Id);
            TempData["SuccessMessage"] = $"Brand {brandvm.Name} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
