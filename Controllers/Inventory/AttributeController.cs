using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Implementation.Inventory;
using ERP_System_Project.Services.Interfaces.Inventory;
using ERP_System_Project.ViewModels.Inventory;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ERP_System_Project.Controllers.Inventory
{
    public class AttributeController : Controller
    {
        private readonly IAttributeService _attributeService;
        private readonly IValidator<AttributeVM> _validator;
        private readonly IMapper _mapper;
        public AttributeController(IAttributeService attributeService, IMapper mapper, IValidator<AttributeVM> validator)
        {
            _attributeService = attributeService;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<IActionResult> Index(int pageNumber, int pageSize, string? searchByName = null)
        {
            var attributes = await _attributeService.GetAllAttributesPaginated(pageNumber, pageSize, searchByName);
            return View(attributes);
        }

        public async Task<IActionResult> New()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(AttributeVM attributeVM)
        {
            var result = _validator.Validate(attributeVM);
            if (result.IsValid)
            {
                var attribute = _mapper.Map<ProductAttribute>(attributeVM);
                await _attributeService.CreateAsync(attribute);
                TempData["SuccessMessage"] = $"Attribute {attributeVM.Name} Created Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(attributeVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var attribute = await _attributeService.GetByIdAsync(id);
            if (attribute is null) return NotFound();
            var attributeVM = _mapper.Map<AttributeVM>(attribute);

            return PartialView(attributeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AttributeVM attributeVM)
        {
            var result = _validator.Validate(attributeVM);

            if (result.IsValid)
            {
                var attribute = await _attributeService.GetByIdAsync(attributeVM.Id);
                _mapper.Map(attributeVM, attribute);
                await _attributeService.UpdateAsync(attribute);
                TempData["SuccessMessage"] = $"Attribute {attributeVM.Name} Updated Successfully";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(attributeVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var attribute = await _attributeService.GetByIdAsync(id);
            if (attribute is null) return NotFound();
            var attributeVM = _mapper.Map<AttributeVM>(attribute);
            return PartialView(attributeVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AttributeVM attributeVM)
        {
            await _attributeService.DeleteAsync(attributeVM.Id);
            TempData["SuccessMessage"] = $"Attribute {attributeVM.Name} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
