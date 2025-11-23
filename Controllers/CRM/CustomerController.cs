using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWork _uow; 
        public CustomerController(ICustomerService customerService, IUnitOfWork uow)
        {
            _customerService = customerService;
            _uow = uow;
        }
        public async Task<IActionResult> Index(
                   int pageNumber = 1,
                   int pageSize = 10,
                    string? searchValue = null,
                   bool includeInactive = false)
        {
            var customers = await _customerService.GetCustomersPaginatedAsync(pageNumber, pageSize, searchValue, includeInactive);
            return View(customers);

        }

        public async Task<IActionResult> CreateAsync()
        {

            await PopulateCustomerTypesDropdown();
            return PartialView();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                // You'll need to add this method to your service
                var result = await _customerService.CreateCustomerVMAsync(customerVM);
                if (result)
                    return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "You have to fill all required fields");

            // Log validation errors for debugging
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            await PopulateCustomerTypesDropdown();

            return View(customerVM);
        }

        public async Task<IActionResult> Details(int id, string viewName = "Details")
        {
            // Use VM method instead of entity method
            var customer = await _customerService.GetCustomerVMByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return PartialView(viewName, customer);
        }

        // Renamed from Update to Edit for consistency with your view
        public async Task<IActionResult> Edit(int id)
        {
            await PopulateCustomerTypesDropdown();
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CustomerVM customerVM)
        {
            if (id != customerVM.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // You'll need to add this method to your service
                var result = await _customerService.UpdateCustomerVMAsync(customerVM);
                if (result)
                    return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "You have to fill all required fields");
            string s = "";
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    s+=error.ErrorMessage;
                }
            }
            await PopulateCustomerTypesDropdown();

            return Content(s);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CustomerVM customerVM)
        {
            if (id != customerVM.Id)
            {
                return BadRequest();
            }

            var result = await _customerService.SoftDeleteCustomerAsync(customerVM.Id);
            if (result)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Something went wrong");
            return View(customerVM);
        }
        private async Task PopulateCustomerTypesDropdown()
        {
            var customerTypes = await _uow.CustomerTypes.GetAllAsync();
            ViewBag.CustomerTypes = customerTypes.Select(ct => new SelectListItem
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();
        }
    }
}
