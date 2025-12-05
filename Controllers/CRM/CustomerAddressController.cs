using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerAddressController : Controller
    {
        private readonly ICustomerAddressService _customerAddressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService; 

        public CustomerAddressController(ICustomerAddressService customerAddressService, UserManager<ApplicationUser> userManager, ICustomerService customerService)
        {
            _customerAddressService = customerAddressService;
            _userManager = userManager;
            _customerService = customerService;
        }

        private async Task<int> GetLoggedInUserCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            var customerId = user?.CustomerId ?? 0;
            Console.WriteLine(customerId);
            return customerId;

        }
        public async Task<IActionResult> Index()
        {

            var customerId = await GetLoggedInUserCustomerId();
            if (customerId == 0) return Unauthorized();
            ViewBag.CustomerId = customerId;

            var addresses = await _customerAddressService.GetAllAddressesVMsByCustomerAsync(customerId);
            return View(addresses);
        }









        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();

            var model = await _customerAddressService.GetAddressByIdAsync(id);
            if (model == null) return NotFound();

            var customerId = await GetLoggedInUserCustomerId();
            if (model.CustomerId != customerId)
                return Forbid();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            int customerId = await GetLoggedInUserCustomerId();
            var vm = new CustomerAddressVM { CustomerId = customerId };
            return PartialView(vm);
        }

        // POST: /CustomerAddress/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerAddressVM model)
        {
            if (model == null) return BadRequest();

            var customerId = await GetLoggedInUserCustomerId();
            if (customerId == 0) return Unauthorized();

            model.CustomerId = customerId; 

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var added = await _customerAddressService.AddAddressAsync(model);
                if (!added)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create address. Please try again.");
                    return View(model);
                }

                TempData["Success"] = "Address created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please contact support.");
                return View(model);
            }
        }

        // GET: /CustomerAddress/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var model = await _customerAddressService.GetAddressByIdAsync(id);
            if (model == null) return NotFound();

            var customerId = await GetLoggedInUserCustomerId();
            if (model.CustomerId != customerId )
                return Forbid();

            return PartialView(model);
        }

        // POST: /CustomerAddress/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerAddressVM model)
        {
            if (model == null || id != model.Id) return BadRequest();

            var customerId = await GetLoggedInUserCustomerId();
            if (customerId == 0) return Unauthorized();

            var existing = await _customerAddressService.GetAddressByIdAsync(id);
            if (existing == null) return NotFound();
            if (existing.CustomerId != customerId)
                return Forbid();

            model.CustomerId = existing.CustomerId;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var updated = await _customerAddressService.EditAddressAsync(customerId, model);
                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "Unable to update address. Please try again.");
                    return View(model);
                }

                TempData["Success"] = "Address updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please contact support.");
                return View(model);
            }
        }

        // POST: /CustomerAddress/Delete/5
        // Using POST for delete to avoid accidental deletes via GET
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var existing = await _customerAddressService.GetAddressByIdAsync(id);
            if (existing == null) return NotFound();

            var customerId = await GetLoggedInUserCustomerId();
            if (customerId == 0) return Unauthorized();

            // Only owner or Admin can delete
            if (existing.CustomerId != customerId && !User.IsInRole("Admin"))
                return Forbid();

            try
            {
                // get all addresses for this customer to check count
                var addresses = await _customerAddressService.GetAllAddressesVMsByCustomerAsync(existing.CustomerId);
                var addressCount = addresses?.Count() ?? 0;

                if (addressCount <= 1)
                {
                    // Customer has only one address -> cannot delete
                    TempData["Error"] = "You cannot delete the only address. Please add another address first if you want to remove this one.";
                    return RedirectToAction(nameof(Index));
                }

                var deleted = await _customerAddressService.RemoveAddressAsync(id);
                if (!deleted)
                {
                    // Common reasons: associated with ongoing orders or internal error
                    TempData["Error"] = "Address could not be deleted. It might be associated with ongoing orders.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                TempData["Success"] = "Address deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred while deleting the address. Please contact support.";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

    }
}
