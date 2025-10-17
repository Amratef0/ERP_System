using AutoMapper;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.Core;
using ERP_System_Project.Services.Interfaces.HR;
using ERP_System_Project.ViewModels.HR;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_System_Project.Controllers.HR
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IJobTitleService _jobTitleService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IValidator<EmployeeVM> _validator;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailSender;
        private readonly IEmployeeLeaveBalanceService _leaveBalanceService;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveTypeService _leaveTypeService;

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
            IEmailService emailSender,
            IEmployeeLeaveBalanceService leaveBalanceService,
            ILeaveRequestService leaveRequestService,
            ILeaveTypeService leaveTypeService)
        {
            _employeeService = employeeService;
            _branchService = branchService;
            _departmentService = departmentService;
            _employeeTypeService = employeeTypeService;
            _jobTitleService = jobTitleService;
            _countryService = countryService;
            _currencyService = currencyService;
            _validator = validator;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _leaveBalanceService = leaveBalanceService;
            _leaveRequestService = leaveRequestService;
            _leaveTypeService = leaveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            EmployeesIndexVM model = new EmployeesIndexVM
            {
                Employees = await _employeeService.GetAllAsync(),
                Branches = await _branchService.GetAllAsync(),
                Departments = await _departmentService.GetAllAsync(),
                EmployeeTypes = await _employeeTypeService.GetAllAsync(),
                JobTitles = await _jobTitleService.GetAllAsync()
            };
            return View("Index", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(string? name, int? branchId, int? departmentId, int? employeeTypeId, int? jobTitleId)
        {
            IEnumerable<EmployeeIndexVM> employees = await _employeeService.SearchAsync(name, branchId, departmentId, employeeTypeId, jobTitleId);
            return Json(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            EmployeeVM model = new EmployeeVM
            {
                Branches = await _branchService.GetAllAsync(),
                Departments = await _departmentService.GetAllAsync(),
                EmployeeTypes = await _employeeTypeService.GetAllAsync(),
                JobTitles = await _jobTitleService.GetAllAsync(),
                Countries = await _countryService.GetAllAsync(),
                Currencies = await _currencyService.GetAllAsync()
            };
            return View("Create", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EmployeeVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);

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

                IdentityResult creationUserResult = await _userManager.CreateAsync(appuser);

                if (creationUserResult.Succeeded)
                {
                    model.ApplicationUserId = appuser.Id;
                    var isCreated = await _employeeService.CreateAsync(model);

                    if (isCreated)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(appuser);

                        var ResetLink = Url.Action("ResetPassword", "Account",
                            new { userEmail = appuser.Email, token = token }, Request.Scheme);

                        await _emailSender.SendEmailAsync(
                            model.WorkEmail,
                            "Reset your password",
                            $"Welcome To Optima ERP System! Please Reset your password by <a href='{ResetLink}'>clicking here</a> to join us."
                        );

                        if (!await _roleManager.RoleExistsAsync("Employee"))
                            await _roleManager.CreateAsync(new IdentityRole("Employee"));

                        await _userManager.AddToRoleAsync(appuser, "Employee");

                        TempData["SuccessMessage"] = $"Employee {model.FirstName} {model.LastName} has been created successfully! Welcome email sent.";
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError(string.Empty, "Cannot add employee!");
                }

                foreach (var error in creationUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Branches = await _branchService.GetAllAsync();
            model.Departments = await _departmentService.GetAllAsync();
            model.EmployeeTypes = await _employeeTypeService.GetAllAsync();
            model.JobTitles = await _jobTitleService.GetAllAsync();
            model.Countries = await _countryService.GetAllAsync();
            model.Currencies = await _currencyService.GetAllAsync();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdWithDetailsAsync(id);

            if (employee == null) return NotFound();

            EmployeeVM model = _mapper.Map<EmployeeVM>(employee);
            model.Branches = await _branchService.GetAllAsync();
            model.Departments = await _departmentService.GetAllAsync();
            model.EmployeeTypes = await _employeeTypeService.GetAllAsync();
            model.JobTitles = await _jobTitleService.GetAllAsync();
            model.Countries = await _countryService.GetAllAsync();
            model.Currencies = await _currencyService.GetAllAsync();

            return View("Edit", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(EmployeeVM model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);
            if (result.IsValid)
            {
                var appuser = await _userManager.FindByIdAsync(model.ApplicationUserId);
                appuser.Email = model.WorkEmail;
                var isUpdated = await _employeeService.UpdateAsync(model);
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Employee {model.FirstName} {model.LastName} has been updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            model.Branches = await _branchService.GetAllAsync();
            model.Departments = await _departmentService.GetAllAsync();
            model.EmployeeTypes = await _employeeTypeService.GetAllAsync();
            model.JobTitles = await _jobTitleService.GetAllAsync();
            model.Countries = await _countryService.GetAllAsync();
            model.Currencies = await _currencyService.GetAllAsync();
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdWithDetailsAsync(id);
            if (employee == null) return NotFound();
            EmployeeVM model = _mapper.Map<EmployeeVM>(employee);
            return View("Details", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Employee not found!";
                return NotFound();
            }

            var appuser = await _userManager.FindByIdAsync(employee.ApplicationUserId);

            if (appuser != null)
            {
                var result = await _userManager.DeleteAsync(appuser);

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = $"Failed to delete employee: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                    return RedirectToAction("Index");
                }
            }

            try
            {
                await _employeeService.DeleteAsync(id);
                TempData["SuccessMessage"] = $"Employee {employee.FirstName} {employee.LastName} has been deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to delete employee: {ex.Message}";
                return BadRequest();
            }
        }

        // ============= EMPLOYEE SELF-SERVICE PORTAL =============

        /// <summary>
        /// Employee Profile Page - Shows employee info, leave balances, and recent requests
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Find employee by ApplicationUserId
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return NotFound("Employee profile not found.");

            // Get leave balances for current year
            var currentYear = DateTime.Now.Year;
            var leaveBalances = await _leaveBalanceService.GetEmployeeBalancesAsync(employee.Id, currentYear);

            // Get recent leave requests
            var recentRequests = await _leaveRequestService.GetEmployeeLeaveRequestsAsync(employee.Id);

            // Build view model
            var model = new EmployeeProfileVM
            {
                Id = employee.Id,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Email = employee.WorkEmail,
                Phone = employee.WorkPhone,
                ImageURL = employee.ImageURL,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                BranchName = employee.Branch?.Name,
                DepartmentName = employee.Department?.Name,
                JobTitleName = employee.JobTitle?.Name,
                EmployeeTypeName = employee.Type?.Name,
                LeaveBalances = leaveBalances,
                RecentLeaveRequests = recentRequests.Take(10),
                PendingRequestsCount = recentRequests.Count(r => r.Status == LeaveRequestStatus.Pending),
                ApprovedRequestsThisYear = recentRequests.Count(r =>
                    r.Status == LeaveRequestStatus.Approved &&
                    r.StartDate.Year == currentYear)
            };

            return View(model);
        }

        /// <summary>
        /// My Leave Requests - Shows all leave requests for logged-in employee
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyLeaveRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return NotFound("Employee profile not found.");

            var leaveRequests = await _leaveRequestService.GetEmployeeLeaveRequestsAsync(employee.Id);

            return View(leaveRequests);
        }

        /// <summary>
        /// Create Leave Request - GET
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> RequestLeave()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return NotFound("Employee profile not found.");

            var model = new LeaveRequestVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                LeaveTypes = await _leaveTypeService.GetAllAsync()
            };

            return View(model);
        }

        /// <summary>
        /// Create Leave Request - POST
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> RequestLeave(LeaveRequestVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return NotFound("Employee profile not found.");

            if (ModelState.IsValid)
            {
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = model.LeaveTypeId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Reason = model.Reason
                };

                var (success, errorMessage) = await _leaveRequestService.CreateLeaveRequestAsync(leaveRequest);

                if (success)
                {
                    TempData["SuccessMessage"] = "Leave request submitted successfully!";
                    return RedirectToAction(nameof(MyLeaveRequests));
                }

                ModelState.AddModelError("", errorMessage ?? "Failed to create leave request.");
            }

            model.LeaveTypes = await _leaveTypeService.GetAllAsync();
            return View(model);
        }

        /// <summary>
        /// Cancel Leave Request
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CancelLeaveRequest(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return NotFound("Employee profile not found.");

            var (success, errorMessage) = await _leaveRequestService.CancelLeaveRequestAsync(id, employee.Id);

            if (success)
                TempData["SuccessMessage"] = "Leave request cancelled successfully.";
            else
                TempData["ErrorMessage"] = errorMessage;

            return RedirectToAction(nameof(MyLeaveRequests));
        }

        /// <summary>
        /// Calculate Leave Days (AJAX)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CalculateLeaveDays(string startDate, string endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return Json(new { success = false, days = 0 });

            if (DateOnly.TryParse(startDate, out var start) && DateOnly.TryParse(endDate, out var end))
            {
                var days = await _leaveRequestService.CalculateLeaveDaysAsync(start, end, employee.Id);
                return Json(new { success = true, days });
            }

            return Json(new { success = false, days = 0 });
        }

        /// <summary>
        /// Get Available Balance for Leave Type (AJAX)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAvailableBalance(int leaveTypeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (employee == null)
                return Json(new { success = false, balance = 0 });

            var currentYear = DateTime.Now.Year;
            var balance = await _leaveBalanceService.GetBalanceAsync(employee.Id, leaveTypeId, currentYear);

            if (balance != null)
                return Json(new { success = true, balance = balance.RemainingDays });

            return Json(new { success = false, balance = 0 });
        }

        // ============= MANAGER APPROVAL PORTAL =============

        /// <summary>
        /// Pending Approvals - Shows all pending leave requests for manager's department
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Manager, Admin")]
        public async Task<IActionResult> PendingApprovals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (manager == null)
                return NotFound("Manager profile not found.");

            // Get pending requests for manager's department (or all if admin)
            int? departmentId = User.IsInRole("Admin") ? null : manager.DepartmentId;
            var pendingRequests = await _leaveRequestService.GetPendingLeaveRequestsAsync(departmentId);

            return View(pendingRequests);
        }

        /// <summary>
        /// Approve Leave Request
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR Manager, Manager, Admin")]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (manager == null)
                return NotFound("Manager profile not found.");

            var (success, errorMessage) = await _leaveRequestService.ApproveLeaveRequestAsync(id, manager.Id);

            if (success)
                TempData["SuccessMessage"] = "Leave request approved successfully!";
            else
                TempData["ErrorMessage"] = errorMessage ?? "Failed to approve leave request.";

            return RedirectToAction(nameof(PendingApprovals));
        }

        /// <summary>
        /// Reject Leave Request
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR Manager, Manager, Admin")]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (manager == null)
                return NotFound("Manager profile not found.");

            var (success, errorMessage) = await _leaveRequestService.RejectLeaveRequestAsync(id, manager.Id);

            if (success)
                TempData["SuccessMessage"] = "Leave request rejected successfully.";
            else
                TempData["ErrorMessage"] = errorMessage ?? "Failed to reject leave request.";

            return RedirectToAction(nameof(PendingApprovals));
        }

        /// <summary>
        /// View Leave Request Details (for managers)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Manager, Admin")]
        public async Task<IActionResult> LeaveRequestDetails(int id)
        {
            var request = await _leaveRequestService.GetByIdWithDetailsAsync(id);

            if (request == null)
                return NotFound();

            // Check if manager has permission to view this request
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (manager != null && !User.IsInRole("Admin"))
            {
                // Managers can only see requests from their department
                if (request.Employee.DepartmentId != manager.DepartmentId)
                    return Forbid();
            }

            return View(request);
        }

        /// <summary>
        /// Team Leave Calendar - Shows all approved leaves for the team
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "HR Manager, Manager, Admin")]
        public async Task<IActionResult> TeamLeaveCalendar()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = await _employeeService.GetByApplicationUserIdAsync(userId);

            if (manager == null)
                return NotFound("Manager profile not found.");

            // Get all approved requests for manager's department
            int? departmentId = User.IsInRole("Admin") ? null : manager.DepartmentId;
            var allRequests = await _leaveRequestService.GetPendingLeaveRequestsAsync(departmentId);

            // Include approved requests as well
            var approvedRequests = allRequests.Where(r =>
                r.Status == LeaveRequestStatus.Approved &&
                r.StartDate >= DateOnly.FromDateTime(DateTime.Today));

            return View(approvedRequests);
        }
    }
}
