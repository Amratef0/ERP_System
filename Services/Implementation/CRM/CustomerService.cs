using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.CRM;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
         // Check if customer already exists
         var existingCustomer = await _uow.Customers
             .GetAllAsIQueryable()
             .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

         if (existingCustomer != null)
             return;

         var customerType = await _uow.CustomerTypes.GetAllAsIQueryable().FirstOrDefaultAsync();

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
             Email = string.IsNullOrWhiteSpace(model.Email) ? user.Email : model.Email,
             PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? user.PhoneNumber : model.PhoneNumber,
             DateOfBirth = user.DateOfBirth,

             RegistrationDate = user.CreatedAt,
             IsActive = true,
             LastLoginDate = DateTime.Now
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




        public async Task<PageSourcePagination<CustomerVM>> GetCustomersPaginatedAsync(
            int pageNumber,
            int pageSize,
            string? searchByName = null,
            bool includeInactive = false)
        {
            Expression<Func<Customer, bool>> filter = c => true;

            if (!includeInactive)
                filter = filter.And(c => c.IsActive);

            if (!string.IsNullOrEmpty(searchByName))
                filter = filter.And(c =>
                    c.FirstName.Contains(searchByName) ||
                    c.LastName.Contains(searchByName) ||
                    c.Email.Contains(searchByName));


            return await _uow.Customers.GetAllPaginatedAsync(
                selector: c => new CustomerVM
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    DateOfBirth = c.DateOfBirth,
                    LastLoginDate = c.LastLoginDate,
                    ModifiedDate = c.ModifiedDate,
                    IsActive = c.IsActive,
                    DeactivatedAt = c.DeactivatedAt,
                    ApplicationUserId = c.ApplicationUserId,
                    CustomerTypeId = c.CustomerTypeId,
                    CustomerTypeName = c.CustomerType != null ? c.CustomerType.Name : null,
                    MainAddress = c.CustomerAddresses
                        .Select(ca => ca.City + ", " + ca.Street + " " + ca.BuildingNumber)
                        .FirstOrDefault(),
                    NumOfOrders = c.Orders.Count()
                },
                expandable: true,
                filter: filter,
                pageNumber: pageNumber,
                pageSize: pageSize,
                Includes: new Expression<Func<Customer, object>>[]
                {
                    c => c.CustomerType,
                    c => c.CustomerAddresses,
                    c => c.Orders
                }
            );
        }
        public async Task<CustomerVM?> GetCustomerVMByIdAsync(int id) { 
        
            var customerVM = await _uow.Customers.GetAllAsIQueryable()
                .Where(c => c.Id == id)
                .Select(c => new CustomerVM
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    DateOfBirth = c.DateOfBirth,
                    LastLoginDate = c.LastLoginDate,
                    ModifiedDate = c.ModifiedDate,
                    IsActive = c.IsActive,
                    DeactivatedAt = c.DeactivatedAt,
                    ApplicationUserId = c.ApplicationUserId,
                    CustomerTypeId = c.CustomerTypeId,
                    CustomerTypeName = c.CustomerType.Name,
                    MainAddress = c.CustomerAddresses.Select(ca => ca.City + ", " + ca.Street + " " + ca.BuildingNumber).FirstOrDefault() ?? "City, Street, Building Number.",
                    NumOfOrders = c.Orders.Count() 

                })
                .FirstOrDefaultAsync();
            return customerVM;
        }


        public async Task<bool> CreateCustomerVMAsync(CustomerVM customerVM)
        {
            try
            {
                var customer = new Customer
                {
                    FirstName = customerVM.FirstName,
                    LastName = customerVM.LastName,
                    Email = customerVM.Email,
                    PhoneNumber = customerVM.PhoneNumber,
                    DateOfBirth = customerVM.DateOfBirth,
                    RegistrationDate = DateTime.Now,
                    IsActive = true,
                    LastLoginDate = DateTime.Now,
                    CustomerTypeId = customerVM.CustomerTypeId,
                    ApplicationUserId = customerVM.ApplicationUserId
                };

                await _uow.Customers.AddAsync(customer);
                return await _uow.CompleteAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error creating customer: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateCustomerVMAsync(CustomerVM customerVM)
        {
            try
            {
                var customer = await _uow.Customers.GetByIdAsync(customerVM.Id);
                if (customer == null) return false;

                // Update properties from VM
                customer.FirstName = customerVM.FirstName;
                customer.LastName = customerVM.LastName;
                customer.Email = customerVM.Email;
                customer.PhoneNumber = customerVM.PhoneNumber;
                customer.DateOfBirth = customerVM.DateOfBirth;
                customer.IsActive = customerVM.IsActive;
                customer.CustomerTypeId = customerVM.CustomerTypeId;
                customer.ModifiedDate = DateTime.Now;
                customer.ApplicationUserId = customerVM.ApplicationUserId;
                customer.LastLoginDate = DateTime.Now; 

                _uow.Customers.Update(customer);
                return await _uow.CompleteAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating customer: {ex.Message}");
                return false;
            }
        }
        public async Task<Customer> GetCustomerByUserIdAsync(string userId)
{
    return await _context.Customers
        .FirstOrDefaultAsync(c => c.UserId == userId);
}




    }
}
