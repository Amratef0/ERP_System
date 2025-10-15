using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.HR
{
    /// <summary>
    /// Controller for managing Employee Leave Balances.
    /// Provides CRUD operations and bulk actions for HR managers.
    /// </summary>
    public class EmployeeLeaveBalanceController : Controller
    {
        private readonly IEmployeeLeaveBalanceService _leaveBalanceService;
        private readonly ILeavePolicyService _leavePolicyService;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IJobTitleService _jobTitleService;

        public EmployeeLeaveBalanceController(
            IEmployeeLeaveBalanceService leaveBalanceService,
            ILeavePolicyService leavePolicyService,
            IEmployeeService employeeService,
            ILeaveTypeService leaveTypeService,
            IBranchService branchService,
            IDepartmentService departmentService,
            IEmployeeTypeService employeeTypeService,
            IJobTitleService jobTitleService)
        {
            _leaveBalanceService = leaveBalanceService;
            _leavePolicyService = leavePolicyService;
            _employeeService = employeeService;
            _leaveTypeService = leaveTypeService;
            _branchService = branchService;
            _departmentService = departmentService;
            _employeeTypeService = employeeTypeService;
            _jobTitleService = jobTitleService;
        }

        // GET: EmployeeLeaveBalance/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new EmployeeLeaveBalanceVM
            {
                LeaveTypes = await _leaveTypeService.GetAllAsync(),
                Branches = await _branchService.GetAllAsync(),
                Departments = await _departmentService.GetAllAsync(),
                EmployeeTypes = await _employeeTypeService.GetAllAsync(),
                JobTitles = await _jobTitleService.GetAllAsync()
            };

            return View(model);
        }

        // POST: EmployeeLeaveBalance/Search (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(
            int? year,
            int? leaveTypeId,
            string? employeeName,
            int? branchId,
            int? departmentId,
            int? employeeTypeId,
            int? jobTitleId)
        {
            var balances = await _leaveBalanceService.GetAllBalancesAsync(
                year,
                leaveTypeId,
                employeeName,
                branchId,
                departmentId,
                employeeTypeId,
                jobTitleId
            );

            return Json(balances);
        }

        // GET: EmployeeLeaveBalance/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
            if (balance == null)
                return NotFound();

            // Get related leave requests for this balance
            var leaveRequests = balance.Employee.LeaveRequests
                .Where(lr => lr.LeaveTypeId == balance.LeaveTypeId && lr.StartDate.Year == balance.Year)
                .OrderByDescending(lr => lr.CreatedDate)
                .ToList();

            ViewBag.LeaveRequests = leaveRequests;

            return View(balance);
        }

        // GET: EmployeeLeaveBalance/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EmployeeLeaveBalanceVM
            {
                Year = DateTime.Now.Year,
                LeaveTypes = await _leaveTypeService.GetAllAsync(),
                Employees = (await _employeeService.GetAllAsync())
                    .Select(e => new Employee
                    {
                        Id = e.Id,
                        FirstName = e.FullName?.Split(' ').FirstOrDefault() ?? "",
                        LastName = e.FullName?.Split(' ').LastOrDefault() ?? ""
                    })
                    .ToList()
            };

            return View(model);
        }

        // POST: EmployeeLeaveBalance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeLeaveBalanceVM model)
        {
            if (ModelState.IsValid)
            {
                // Check if balance already exists
                if (await _leaveBalanceService.BalanceExistsAsync(model.EmployeeId, model.LeaveTypeId, model.Year))
                {
                    ModelState.AddModelError("", $"A balance already exists for this employee, leave type, and year.");
                }
                else
                {
                    var isCreated = await _leaveBalanceService.CreateBalanceAsync(model);
                    if (isCreated)
                    {
                        TempData["SuccessMessage"] = "Leave balance created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Failed to create leave balance.");
                }
            }

            // Reload dropdown data
            model.LeaveTypes = await _leaveTypeService.GetAllAsync();
            model.Employees = (await _employeeService.GetAllAsync())
                .Select(e => new Employee
                {
                    Id = e.Id,
                    FirstName = e.FullName?.Split(' ').FirstOrDefault() ?? "",
                    LastName = e.FullName?.Split(' ').LastOrDefault() ?? ""
                })
                .ToList();

            return View(model);
        }

        // GET: EmployeeLeaveBalance/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
            if (balance == null)
                return NotFound();

            var model = new EmployeeLeaveBalanceVM
            {
                Id = balance.Id,
                EmployeeId = balance.EmployeeId,
                LeaveTypeId = balance.LeaveTypeId,
                Year = balance.Year,
                TotalEntitledDays = balance.TotalEntitledDays,
                UsedDays = balance.UsedDays,
                RemainingDays = balance.RemainingDays,
                EmployeeName = $"{balance.Employee.FirstName} {balance.Employee.LastName}",
                LeaveTypeName = balance.LeaveType.Name
            };

            return View(model);
        }

        // POST: EmployeeLeaveBalance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeLeaveBalanceVM model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Only allow updating TotalEntitledDays
                var isUpdated = await _leaveBalanceService.UpdateBalanceEntitlementAsync(
                    model.Id,
                    model.TotalEntitledDays
                );

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Leave balance updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update leave balance.");
            }

            // Reload employee and leave type names for display
            var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
            if (balance != null)
            {
                model.EmployeeName = $"{balance.Employee.FirstName} {balance.Employee.LastName}";
                model.LeaveTypeName = balance.LeaveType.Name;
            }

            return View(model);
        }

        // POST: EmployeeLeaveBalance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
            if (balance == null)
                return NotFound();

            // Warning if UsedDays > 0
            if (balance.UsedDays > 0)
            {
                TempData["WarningMessage"] = $"This balance has {balance.UsedDays} used days. Deleting may affect leave request history.";
            }

            var isDeleted = await _leaveBalanceService.DeleteAsync(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Leave balance deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to delete leave balance.";
            return RedirectToAction(nameof(Index));
        }

        // GET: EmployeeLeaveBalance/GenerateBalances
        [HttpGet]
        public async Task<IActionResult> GenerateBalances()
        {
            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.NextYear = DateTime.Now.Year + 1;
            ViewBag.LeaveTypes = await _leaveTypeService.GetAllAsync();
            return View();
        }

        // POST: EmployeeLeaveBalance/GenerateForAllEmployees
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateForAllEmployees(int year, int? leaveTypeId)
        {
            var isGenerated = await _leaveBalanceService.GenerateBalancesForAllEmployeesAsync(year, leaveTypeId);

            if (isGenerated)
            {
                var message = leaveTypeId.HasValue
                    ? $"Balances generated for all employees for year {year} and selected leave type."
                    : $"Balances generated for all employees for year {year} (all leave types).";

                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to generate balances.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EmployeeLeaveBalance/CarryForward
        [HttpGet]
        public IActionResult CarryForward()
        {
            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.PreviousYear = DateTime.Now.Year - 1;
            return View();
        }

        // POST: EmployeeLeaveBalance/ExecuteCarryForward
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteCarryForward(int fromYear, int toYear)
        {
            if (fromYear >= toYear)
            {
                TempData["ErrorMessage"] = "From year must be before To year.";
                return RedirectToAction(nameof(CarryForward));
            }

            var isCarriedForward = await _leaveBalanceService.CarryForwardUnusedDaysAsync(fromYear, toYear);

            if (isCarriedForward)
            {
                TempData["SuccessMessage"] = $"Successfully carried forward unused balances from {fromYear} to {toYear}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to carry forward balances.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EmployeeLeaveBalance/GetEntitledDays (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetEntitledDays(int employeeId, int leaveTypeId)
        {
            try
            {
                var entitledDays = await _leavePolicyService.CalculateEntitledDaysAsync(employeeId, leaveTypeId);
                return Json(new { success = true, entitledDays });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, entitledDays = 0, error = ex.Message });
            }
        }
    }
}
