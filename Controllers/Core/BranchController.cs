using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.ViewModels.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Controllers.Core
{
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly IValidator<BranchVM> _validator;

        public BranchController(IBranchService branchService,
            ICountryService countryService,
            IMapper mapper,
            IValidator<BranchVM> validator)
        {
            _branchService = branchService;
            _countryService = countryService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BranchesIndexVM model = new BranchesIndexVM()
            {
                Branches = await _branchService.GetAllAsync(),
                Countries = await _countryService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BranchVM model = new BranchVM()
            {
                Countries = await _countryService.GetAllAsync()
            };
            return View("Create", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BranchVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                Branch branch = _mapper.Map<Branch>(model);
                await _branchService.CreateAsync(branch);
                TempData["SuccessMessage"] = $"Branch '{model.Name}' has been created successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await _countryService.GetAllAsync();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var branch = await _branchService.GetWithAddressAsync(id);
            if (branch == null)
            {
                TempData["ErrorMessage"] = "Branch not found!";
                return NotFound();
            }

            BranchVM model = _mapper.Map<BranchVM>(branch);
            model.Country = branch.Address.Country.Name;

            return View("Details", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Branch branch = await _branchService.GetWithAddressAsync(id);
            if (branch == null)
            {
                TempData["ErrorMessage"] = "Branch not found!";
                return NotFound();
            }

            BranchVM model = _mapper.Map<BranchVM>(branch);
            model.Countries = await _countryService.GetAllAsync();
            model.Country = branch.Address.Country.Name;

            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(BranchVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                Branch branch = _mapper.Map<Branch>(model);
                await _branchService.UpdateAsync(branch);
                TempData["SuccessMessage"] = $"Branch '{model.Name}' has been updated successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await _countryService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Branch branch = await _branchService.GetByIdAsync(id);
            if (branch == null)
            {
                TempData["ErrorMessage"] = "Branch not found!";
                return NotFound();
            }

            try
            {
                await _branchService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Branch '{branch.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                // Log the full exception for debugging
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");

                TempData["ErrorMessage"] = $"Cannot delete branch '{branch.Name}' because it is being used by other records (employees, warehouses, inventory transactions, etc.).";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete branch '{branch.Name}': {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred while deleting branch '{branch.Name}': {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Filter(string name, int countryId)
        {
            var branches = await _branchService.FilterAsync(name, countryId);
            return Json(branches);
        }
    }
}
