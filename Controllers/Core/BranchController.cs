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
            try
            {
                BranchesIndexVM model = new BranchesIndexVM()
                {
                    Branches = await _branchService.GetAllAsync(),
                    Countries = await _countryService.GetAllAsync()
                };
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branches. Please try again.";
                return View("Index", new BranchesIndexVM { Branches = new List<Branch>(), Countries = new List<Country>() });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                BranchVM model = new BranchVM()
                {
                    Countries = await _countryService.GetAllAsync()
                };
                return View("Create", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BranchVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    Branch branch = _mapper.Map<Branch>(model);
                    await _branchService.CreateAsync(branch);
                    TempData["SuccessMessage"] = $"Branch '{model.Name}' has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A branch with the name '{model.Name}' already exists.");
                        TempData["ErrorMessage"] = "This branch name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create branch due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the branch.";
                }
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            model.Countries = await _countryService.GetAllAsync();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branch details.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the branch.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(BranchVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

            if (result.IsValid)
            {
                try
                {
                    Branch branch = _mapper.Map<Branch>(model);
                    await _branchService.UpdateAsync(branch);
                    TempData["SuccessMessage"] = $"Branch '{model.Name}' has been updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _branchService.GetByIdAsync(model.Id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This branch has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This branch was modified by another user.");
                        TempData["WarningMessage"] = "The branch was modified by another user. Please refresh and try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("Name", $"A branch with the name '{model.Name}' already exists.");
                        TempData["ErrorMessage"] = "This branch name is already in use.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update branch due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the branch.";
                }
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            model.Countries = await _countryService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Branch branch = await _branchService.GetByIdAsync(id);
                if (branch == null)
                {
                    TempData["ErrorMessage"] = "Branch not found!";
                    return NotFound();
                }

                await _branchService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Branch '{branch.Name}' has been deleted successfully!";
            }
            catch (DbUpdateException ex)
            {
                var branch = await _branchService.GetByIdAsync(id);
                var branchName = branch?.Name ?? "this branch";
                
                if (ex.InnerException != null && 
                    (ex.InnerException.Message.Contains("REFERENCE constraint") || 
                     ex.InnerException.Message.Contains("FOREIGN KEY constraint") ||
                     ex.InnerException.Message.Contains("DELETE statement conflicted")))
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{branchName}' because it is being used by employees, warehouses, inventory transactions, or other records. Please reassign or remove these associations first.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Cannot delete '{branchName}' due to a database error.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = $"Cannot delete this branch: {ex.Message}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the branch.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Filter(string name, int countryId)
        {
            try
            {
                var branches = await _branchService.FilterAsync(name, countryId);
                return Json(branches);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to filter branches." });
            }
        }
    }
}
