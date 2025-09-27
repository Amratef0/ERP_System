using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ERP_System_Project.Controllers.Inventory
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var brands = await _categoryService.GetCategoriesPaginated(pageNumber,pageSize,searchByName);
            return View(brands);
        }

        public async Task<IActionResult> New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryVM);
                await _categoryService.CreateAsync(category);
                return RedirectToAction("Index");
            }
            return View(categoryVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null) return NotFound();
            var categoryVM = _mapper.Map<CategoryVM>(category);

            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryService.GetByIdAsync(categoryVM.Id);
                _mapper.Map(categoryVM, category);
                await _categoryService.UpdateAsync(category);
                return RedirectToAction("Index");
            }
            return View(categoryVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null) return NotFound();
            var categoryVM = _mapper.Map<CategoryVM>(category);
            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryVM categoryVM)
        {
            await _categoryService.DeleteAsync(categoryVM.Id);
            return RedirectToAction("Index");
        }
    }
}
