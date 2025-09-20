using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces.CRM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            var customers =await _customerService.GetAllAsync();
            return View(customers);
        }

        public IActionResult Create()
        {

            // send data with ViewBag or ViewData if needed for dropdowns
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {

            if (ModelState.IsValid)
            {
              var result =  await _customerService.CreateAsync(customer);
                if (result)
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "You have to fill all required fields");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(customer);
        }
        public async Task<IActionResult> Details (int id , string viewName = "Details")
        {
            var customer =await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(viewName, customer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                var result = await _customerService.UpdateAsync(customer);
                if (result)
                    return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "You have to fill all required fields");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(customer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            var result = await _customerService.SoftDeleteCustomerAsync(customer.Id);
            if (result)
                return RedirectToAction("Index");
            ModelState.AddModelError("", "Something went wrong");
            return View(customer);
        }

    }
}
