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
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading leave balances. Please try again.";
                return View(new EmployeeLeaveBalanceVM());
            }
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
            try
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
            catch (Exception ex)
            {
                return Json(new { error = "Failed to search leave balances." });
            }
        }

        // GET: EmployeeLeaveBalance/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
                if (balance == null)
                {
                    TempData["ErrorMessage"] = "Leave balance not found!";
                    return NotFound();
                }

                // Get related leave requests for this balance
                var leaveRequests = balance.Employee.LeaveRequests
                    .Where(lr => lr.LeaveTypeId == balance.LeaveTypeId && lr.StartDate.Year == balance.Year)
                    .OrderByDescending(lr => lr.CreatedDate)
                    .ToList();

                ViewBag.LeaveRequests = leaveRequests;

                return View(balance);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading leave balance details.";
                return RedirectToAction("Index");
            }
        }

        // GET: EmployeeLeaveBalance/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction("Index");
            }
        }

        // POST: EmployeeLeaveBalance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeLeaveBalanceVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if balance already exists
                    if (await _leaveBalanceService.BalanceExistsAsync(model.EmployeeId, model.LeaveTypeId, model.Year))
                    {
                        ModelState.AddModelError("", $"A balance already exists for this employee, leave type, and year.");
                        TempData["ErrorMessage"] = "A leave balance already exists for this employee and leave type in this year.";
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
                        TempData["ErrorMessage"] = "Failed to create leave balance. Please try again.";
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("duplicate key"))
                    {
                        ModelState.AddModelError("", "A balance with these details already exists.");
                        TempData["ErrorMessage"] = "This leave balance already exists.";
                    }
                    else if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("FOREIGN KEY"))
                    {
                        ModelState.AddModelError("", "Invalid employee or leave type selected.");
                        TempData["ErrorMessage"] = "Invalid employee or leave type selected.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to create leave balance due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the leave balance.";
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
            try
            {
                var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
                if (balance == null)
                {
                    TempData["ErrorMessage"] = "Leave balance not found!";
                    return NotFound();
                }

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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the leave balance.";
                return RedirectToAction("Index");
            }
        }

        // POST: EmployeeLeaveBalance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeLeaveBalanceVM model)
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "Invalid request.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
                    TempData["ErrorMessage"] = "Failed to update leave balance. Please try again.";
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    var exists = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
                    if (exists == null)
                    {
                        TempData["ErrorMessage"] = "This leave balance has been deleted by another user.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This leave balance was modified by another user.");
                        TempData["WarningMessage"] = "The leave balance was modified by another user. Please refresh and try again.";
                    }
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("CHECK constraint"))
                    {
                        ModelState.AddModelError("TotalEntitledDays", "Entitled days must be greater than or equal to used days.");
                        TempData["ErrorMessage"] = "Entitled days cannot be less than already used days.";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                        TempData["ErrorMessage"] = "Failed to update leave balance due to a database error.";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the leave balance.";
                }
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
            try
            {
                var balance = await _leaveBalanceService.GetByIdWithDetailsAsync(id);
                if (balance == null)
                {
                    TempData["ErrorMessage"] = "Leave balance not found!";
                    return NotFound();
                }

                // Warning if UsedDays > 0
                if (balance.UsedDays > 0)
                {
                    TempData["WarningMessage"] = $"This balance has {balance.UsedDays} used days. Deleting may affect leave request history.";
                }

                var isDeleted = await _leaveBalanceService.DeleteAsync(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Leave balance deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete leave balance.";
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && 
                    (dbEx.InnerException.Message.Contains("REFERENCE constraint") || 
                     dbEx.InnerException.Message.Contains("FOREIGN KEY constraint")))
                {
                    TempData["ErrorMessage"] = "Cannot delete this leave balance because it has associated leave requests. Please delete the leave requests first.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete leave balance due to a database error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the leave balance.";
            }

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
