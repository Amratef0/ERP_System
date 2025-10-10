using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerService :GenericService<Customer>, ICustomerService
    {
        private readonly IUnitOfWork _uow;


        public CustomerService(IUnitOfWork uow) : base(uow)
        {

            _uow = uow;
        }




        public async Task CreateCustomerByApplicationUserAsync(ApplicationUser user, RegisterViewModel model)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                // Get customer types and use the first active one
                var customerType= await _uow.CustomerTypes.GetAllAsIQueryable().FirstOrDefaultAsync();


                if (customerType == null)
                    throw new InvalidOperationException("No customer types available in the system");

                var customer = new Customer
                {
                    ApplicationUserId = user.Id,
                    ApplicationUser = user,

                    CustomerTypeId = customerType.Id,
                    CustomerType = customerType,

                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = string.IsNullOrWhiteSpace(model.Email) ? user.Email: model.Email,
                    PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? user.PhoneNumber : model.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,

                    RegistrationDate = user.CreatedAt,
                    IsActive = true,
                    LastLoginDate = DateTime.Now,
                    ModifiedDate = null,
                    DeactivatedAt = null
                };

                var customerAddress = new CustomerAddress
                {
                    Customer = customer, 
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street,
                    BuildingNumber = model.BuildingNumber,
                    ApartmentNumber = model.ApartmentNumber
                };
                customer.CustomerAddresses.Add(customerAddress);

                await _uow.Customers.AddAsync(customer);
                await _uow.CompleteAsync();
                user.CustomerId = customer.Id; 

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create customer for user {user.Email}: {ex.Message}", ex);
            }
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
