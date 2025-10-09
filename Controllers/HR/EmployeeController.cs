using AutoMapper;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
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

        public EmployeeController(
            IEmployeeService employeeService,
            IBranchService branchService,
            IDepartmentService departmentService,
            IEmployeeTypeService employeeTypeService,
            IJobTitleService jobTitleService,
            ICountryService countryService,
            ICurrencyService currencyService,
            IValidator<EmployeeVM> validator,
            IMapper mapper)
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
            EmployeesIndexVM model = new EmployeesIndexVM
            {
                Employees = await employeeService.SearchAsync(name, branchId, departmentId, employeeTypeId, jobTitleId),
                Branches = await branchService.GetAllAsync(),
                Departments = await departmentService.GetAllAsync(),
                EmployeeTypes = await employeeTypeService.GetAllAsync(),
                JobTitles = await jobTitleService.GetAllAsync()
            };
            return View("Index", model);
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
                var isCreated = await employeeService.CreateAsync(model);
                if (isCreated)
                    return RedirectToAction("Index");
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
