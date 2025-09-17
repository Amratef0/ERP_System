using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository) { 
        _customerRepository = customerRepository;
        }

        public async Task<Customer?> GetByIdAsync(int id, bool includeInactive = false)
        {
            if (includeInactive)
            
                return await _customerRepository.GetByIdAsync(id);
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null && !customer.IsActive) return null;
            return customer;
        }

        public async Task<bool> ReactivateCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;
            if (customer.IsActive) return true;

            customer.IsActive = true;
            customer.DeactivatedAt = null;
            customer.ModifiedDate = DateTime.Now;
            _customerRepository.Update(customer);
            await _customerRepository.SaveAsync();
            return true;


        }

        public async Task<bool> SoftDeleteCustomerAsync(int id) { 
        var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;
            if (!customer.IsActive) return true;
            
            customer.IsActive = false;
                customer.DeactivatedAt = DateTime.Now;
                customer.ModifiedDate = DateTime.Now;
                _customerRepository.Update(customer);
                await _customerRepository.SaveAsync();
                return true;

        }





    }
}
