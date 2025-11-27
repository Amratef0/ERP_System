using AutoMapper;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.HR;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP_System_Project.Controllers.CRM
{
    public class CustomerTypeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CustomerTypeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var customerTypes = await _uow.CustomerTypes.GetAllAsync();
            var customers = await _uow.Customers.GetAllAsync();

            var counts = customers.Where(c=>c.CustomerTypeId.HasValue)
                .GroupBy(c=>c.CustomerTypeId!.Value)
                .ToDictionary(g=>g.Key, g=>g.Count());
            var vms = _mapper.Map<IEnumerable<CustomerTypeVM>>(customerTypes).ToList();


            foreach (var vm in vms)
            {
                counts.TryGetValue(vm.Id, out var cnt);
                vm.CustomerCount = cnt;
            }
            return View(vms);
        }
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            var existing = await _uow.CustomerTypes.AnyAsync(ct => ct.Name == model.Name);
            if (existing)
            {
                ModelState.AddModelError(nameof(model.Name), "A customer type with the same Name already exists.");
                return PartialView(model);
            }

            try
            {
                var customerType = _mapper.Map<CustomerType>(model);
                await _uow.CustomerTypes.AddAsync(customerType);
                await _uow.CompleteAsync();

                TempData["Success"] = "Customer type created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the customer type.");
                ModelState.AddModelError(string.Empty, ex.Message); // temporary for debugging
                return PartialView(model);
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();
            var customerType = await _uow.CustomerTypes.GetByIdAsync((int)id);
            if (customerType is null)
                return NotFound();

            var mappedType = _mapper.Map<CustomerType, CustomerTypeVM>(customerType);

            return PartialView(mappedType);
        }
        public async Task<IActionResult> Edit(int id)
        {

            var customerType = await _uow.CustomerTypes.GetByIdAsync(id);
            if (customerType == null) return NotFound();

            var vm = _mapper.Map<CustomerTypeVM>(customerType);
            vm.CustomerCount = await _uow.Customers.Count(c => c.CustomerTypeId == id);

            return PartialView(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CustomerTypeVM model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {

                var exist = await _uow.CustomerTypes.AnyAsync(ct => ct.Name == model.Name && ct.Id != id);
                if (exist)
                {
                    ModelState.AddModelError(nameof(model.Name), "Another customer type with this name already exists.");
                    return View(model);
                }

                var entity = await _uow.CustomerTypes.GetByIdAsync(id);
                if (entity == null) return NotFound();
                try
                {
                    // map VM onto loaded entity (preserve nav props)
                    var customerType = _mapper.Map(model,entity);
                    _uow.CustomerTypes.Update(customerType);
                    await _uow.CompleteAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the customer type. Please try again.");

                }

            }
            return View(model);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var customerType = await _uow.CustomerTypes.GetByIdAsync(id);
            if (customerType == null) return NotFound();
            var vm = _mapper.Map<CustomerTypeVM>(customerType);
            vm.CustomerCount = await _uow.Customers.Count(c => c.CustomerTypeId == id);
            return PartialView(vm);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var entity = await _uow.CustomerTypes.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var isUsed = await _uow.Customers.AnyAsync(c=>c.CustomerTypeId == id);
            if (isUsed)
                {
                ModelState.AddModelError(string.Empty, "Cannot delete this type because it is assigned to customers.");
                var vm = _mapper.Map<CustomerTypeVM>(entity);
                vm.CustomerCount = await _uow.Customers.Count(c => c.CustomerTypeId == id);
                return View(vm);
            }

            try
            {
                _uow.CustomerTypes.Delete(id);
                await _uow.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the customer type. Please try again.");

                var vm = _mapper.Map<CustomerTypeVM>(entity);
                return View(vm);
            }



        }
    }
}
