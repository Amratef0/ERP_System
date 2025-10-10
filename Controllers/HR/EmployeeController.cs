using AutoMapper;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IBranchService branchService;
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeTypeService employeeTypeService;
        private readonly IJobTitleService jobTitleService;
        private readonly ICountryService countryService;
        private readonly ICurrencyService currencyService;
        private readonly IValidator<EmployeeVM> validator;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailService emailSender;

        public EmployeeController(
            IEmployeeService employeeService,
            IBranchService branchService,
            IDepartmentService departmentService,
            IEmployeeTypeService employeeTypeService,
            IJobTitleService jobTitleService,
            ICountryService countryService,
            ICurrencyService currencyService,
            IValidator<EmployeeVM> validator,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailSender)
        {
            this.employeeService = employeeService;
            this.branchService = branchService;
            this.departmentService = departmentService;
            this.employeeTypeService = employeeTypeService;
            this.jobTitleService = jobTitleService;
            this.countryService = countryService;
            this.currencyService = currencyService;
            this.validator = validator;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            EmployeesIndexVM model = new EmployeesIndexVM
            {
                Employees = await employeeService.GetAllAsync(),
                Branches = await branchService.GetAllAsync(),
                Departments = await departmentService.GetAllAsync(),
                EmployeeTypes = await employeeTypeService.GetAllAsync(),
                JobTitles = await jobTitleService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SearchAsync(string? name, int? branchId, int? departmentId, int? employeeTypeId, int? jobTitleId)
        {
            IEnumerable<EmployeeIndexVM> employees = await employeeService.SearchAsync(name, branchId, departmentId, employeeTypeId, jobTitleId);
            return Json(employees);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            EmployeeVM model = new EmployeeVM
            {
                Branches = await branchService.GetAllAsync(),
                Departments = await departmentService.GetAllAsync(),
                EmployeeTypes = await employeeTypeService.GetAllAsync(),
                JobTitles = await jobTitleService.GetAllAsync(),
                Countries = await countryService.GetAllAsync(),
                Currencies = await currencyService.GetAllAsync()
            };
            return View("Add", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddAsync(EmployeeVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);

            if (result.IsValid)
            {
                var appuser = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = $"{model.FirstName} {model.LastName}",
                    Email = model.WorkEmail,
                    PhoneNumber = model.WorkPhone,
                    DateOfBirth = model.DateOfBirth,
                    CreatedAt = DateTime.Now,
                };

                IdentityResult creationUserResult = await userManager.CreateAsync(appuser);

                if (creationUserResult.Succeeded)
                {
                    model.ApplicationUserId = appuser.Id;
                    var isCreated = await employeeService.CreateAsync(model);

                    if (isCreated)
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(appuser);

                        var ResetLink = Url.Action("ResetPassword", "Account",
                            new { userEmail = appuser.Email, token = token }, Request.Scheme);

                        await emailSender.SendEmailAsync(
                            model.WorkEmail,
                            "Reset your password",
                            $"Welcome To Optima ERP System! Please Reset your password by <a href='{ResetLink}'>clicking here</a> to join us."
                        );

                        if (!await roleManager.RoleExistsAsync("Employee"))
                            await roleManager.CreateAsync(new IdentityRole("Employee"));

                        await userManager.AddToRoleAsync(appuser, "Employee");

                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError(string.Empty, "Cannot add employee!");
                }

                foreach (var error in creationUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Branches = await branchService.GetAllAsync();
            model.Departments = await departmentService.GetAllAsync();
            model.EmployeeTypes = await employeeTypeService.GetAllAsync();
            model.JobTitles = await jobTitleService.GetAllAsync();
            model.Countries = await countryService.GetAllAsync();
            model.Currencies = await currencyService.GetAllAsync();
            return View("Add", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var employee = await employeeService.GetByIdWithDetailsAsync(id);

            if (employee == null) return NotFound();

            EmployeeVM model = mapper.Map<EmployeeVM>(employee);
            model.Branches = await branchService.GetAllAsync();
            model.Departments = await departmentService.GetAllAsync();
            model.EmployeeTypes = await employeeTypeService.GetAllAsync();
            model.JobTitles = await jobTitleService.GetAllAsync();
            model.Countries = await countryService.GetAllAsync();
            model.Currencies = await currencyService.GetAllAsync();

            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAsync(EmployeeVM model)
        {
            ValidationResult result = await validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var appuser = await userManager.FindByIdAsync(model.ApplicationUserId);
                appuser.Email = model.WorkEmail;
                var isUpdated = await employeeService.UpdateAsync(model);
                if (isUpdated)
                    return RedirectToAction("Index");
            }
            model.Branches = await branchService.GetAllAsync();
            model.Departments = await departmentService.GetAllAsync();
            model.EmployeeTypes = await employeeTypeService.GetAllAsync();
            model.JobTitles = await jobTitleService.GetAllAsync();
            model.Countries = await countryService.GetAllAsync();
            model.Currencies = await currencyService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var employee = await employeeService.GetByIdWithDetailsAsync(id);
            if (employee == null) return NotFound();
            EmployeeVM model = mapper.Map<EmployeeVM>(employee);
            return View("Details", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var appuser = await userManager.FindByIdAsync(employee.ApplicationUserId);

            if (appuser != null)
            {
                var result = await userManager.DeleteAsync(appuser);

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = string.Join(", ", result.Errors.Select(e => e.Description));

                    return RedirectToAction("Index");
                }
            }

            try
            {

                await employeeService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
