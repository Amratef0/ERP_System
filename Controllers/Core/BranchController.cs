using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.ViewModels.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.Core
{
    public class BranchController : Controller
    {
        private readonly IBranchService branchService;
        private readonly ICountryService countryService;
        private readonly IMapper mapper;
        private readonly IValidator<BranchVM> validator;

        public BranchController(IBranchService branchService,
            ICountryService countryService,
            IMapper mapper,
            IValidator<BranchVM> validator)
        {
            this.branchService = branchService;
            this.countryService = countryService;
            this.mapper = mapper;
            this.validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            BranchesIndexVM model = new BranchesIndexVM()
            {
                Branches = await branchService.GetAllAsync(),
                Countries = await countryService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            BranchVM model = new BranchVM()
            {
                Countries = await countryService.GetAllAsync()
            };
            return View("Add", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddAsync(BranchVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                Branch branch = mapper.Map<Branch>(model);
                await branchService.CreateAsync(branch);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await countryService.GetAllAsync();
            return View("Add", model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var branch = await branchService.GetWithAddressAsync(id);
            if (branch == null) return NotFound();

            BranchVM model = mapper.Map<BranchVM>(branch);
            model.Country = branch.Address.Country.Name;

            return View("Details", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            Branch branch = await branchService.GetWithAddressAsync(id);
            if (branch == null) return NotFound();

            BranchVM model = mapper.Map<BranchVM>(branch);
            model.Countries = await countryService.GetAllAsync();
            model.Country = branch.Address.Country.Name;

            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAsync(BranchVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                Branch branch = mapper.Map<Branch>(model);
                await branchService.UpdateAsync(branch);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            model.Countries = await countryService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Branch branch = await branchService.GetByIdAsync(id);
            if (branch == null) return NotFound();
            try
            {
                await branchService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Cannot delete this branch because it is referenced by other records.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> FilterAsync(string name, int countryId)
        {
            var branches = await branchService.FilterAsync(name, countryId);
            return Json(branches);
        }
    }
}
