using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Core;
using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ERP_System_Project.Controllers.Inventory
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryVM> _validator;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper, IValidator<CategoryVM> validator)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var brands = await _categoryService.GetCategoriesPaginated(pageNumber,pageSize,searchByName);
            return View(brands);
        }

        public async Task<IActionResult> New()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(CategoryVM categoryVM)
        {
            var result = _validator.Validate(categoryVM);
            if (result.IsValid)
            {
                var category = _mapper.Map<Category>(categoryVM);
                await _categoryService.CreateAsync(category);
                TempData["SuccessMessage"] = $"Category {categoryVM.Name} Created Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(categoryVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null) return NotFound();
            var categoryVM = _mapper.Map<CategoryVM>(category);

            return PartialView(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM categoryVM)
        {
            var result = _validator.Validate(categoryVM);

            if (result.IsValid)
            {
                var category = await _categoryService.GetByIdAsync(categoryVM.Id);
                _mapper.Map(categoryVM, category);
                await _categoryService.UpdateAsync(category);
                TempData["SuccessMessage"] = $"Category {category.Name} Updated Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(categoryVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null) return NotFound();
            var categoryVM = _mapper.Map<CategoryVM>(category);
            return PartialView(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryVM categoryVM)
        {
            await _categoryService.DeleteAsync(categoryVM.Id);
            TempData["SuccessMessage"] = $"Category {categoryVM.Name} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
