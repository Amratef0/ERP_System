using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerService :GenericService<Customer>, ICustomerService
    {
        private readonly IUnitOfWork _uow;

        public CustomerService(IUnitOfWork uow) : base(uow)
        {

            _uow = uow;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(bool includeInactive = false)
        {
            if (includeInactive)
            {
                return await _uow.Customers.GetAllAsync();
            }
            else
            {
                var customers = await _uow.Customers.GetAllAsync();
                return customers.Where(c => c.IsActive);
            }
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id, bool includeInactive = false) {

            var customer = await _uow.Customers.GetByIdAsync(id);
            if (customer == null) return null;

            if (!includeInactive && !customer.IsActive)
                return null;

            return customer;



        }




        public async Task<bool> ReactivateCustomerAsync(int id)
        {
            try
            {
                var customer = await _uow.Customers.GetByIdAsync(id);
                if (customer == null) return false;
                if (customer.IsActive) return true;

                customer.IsActive = true;
                customer.DeactivatedAt = null;
                customer.ModifiedDate = DateTime.Now;
                _uow.Customers.Update(customer);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteCustomerAsync(int id)
        {

            try
            {
                var customer = await _uow.Customers.GetByIdAsync(id);
                if (customer == null) return false;
                if (!customer.IsActive) return true;

                customer.IsActive = false;
                customer.DeactivatedAt = DateTime.Now;
                customer.ModifiedDate = DateTime.Now;
                _uow.Customers.Update(customer);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }




        }
    }
}
