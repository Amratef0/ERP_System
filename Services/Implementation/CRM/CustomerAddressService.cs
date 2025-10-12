using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels.CRM;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.CRM
{
    public class CustomerAddressService : GenericService<CustomerAddress>, ICustomerAddressService
    {
        private readonly IUnitOfWork _uow;

        public CustomerAddressService(IUnitOfWork uow) : base(uow)
        {

            _uow = uow;
        }

        public async Task<IEnumerable<CustomerAddressVM>> GetAllAddressesVMsByCustomerAsync(int customerId)
        {
            var addresses = await _uow.CustomerAddresses.GetAllAsIQueryable()
                    .Where(ca => ca.CustomerId == customerId)
                    .Select(ca => new CustomerAddressVM
                    {
                        Id = ca.Id,
                        CustomerId = ca.CustomerId,
                        City = ca.City,
                        Country = ca.Country,
                        ApartmentNumber = ca.ApartmentNumber,
                        BuildingNumber = ca.BuildingNumber,
                        Street = ca.Street,
                        NumOfBillingOrders = ca.OrderBillingAddresses.Count(),
                        NumOfShippingOrders = ca.OrderShippingAddresses.Count()
                    })
                    .ToListAsync();

            return addresses; ;


        }
        public async Task<CustomerAddressVM> GetAddressByIdAsync(int id) { 
        var address = await _uow.CustomerAddresses.GetAllAsIQueryable()
                .Where(ca => ca.Id == id)
                .Select(ca => new CustomerAddressVM
                {
                    Id = ca.Id,
                    CustomerId = ca.CustomerId,
                    City = ca.City,
                    Country = ca.Country,
                    ApartmentNumber = ca.ApartmentNumber,
                    BuildingNumber = ca.BuildingNumber,
                    Street = ca.Street,
                    NumOfBillingOrders = ca.OrderBillingAddresses.Count,
                    NumOfShippingOrders = ca.OrderShippingAddresses.Count
                }).FirstOrDefaultAsync();
            return address!;

        }
        public async Task<bool> AddAddressAsync(CustomerAddressVM model) { 
        
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Address model cannot be null");
                var customer = await _uow.Customers.GetByIdAsync(model.CustomerId);
                if (customer == null)
                    throw new ArgumentException("Customer not found");
                var address = new CustomerAddress
                {
                    CustomerId = model.CustomerId,
                    City = model.City,
                    Country = model.Country,
                    ApartmentNumber = model.ApartmentNumber,
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                };
                await _uow.CustomerAddresses.AddAsync(address);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> EditAddressAsync(int customerId, CustomerAddressVM model) { 
        try {
                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Address model cannot be null");
                var address = await _uow.CustomerAddresses.GetByIdAsync(model.Id);
                if (address == null || address.CustomerId != customerId)
                    throw new ArgumentException("Address not found or does not belong to the customer");
                address.City = model.City;
                address.Country = model.Country;
                address.ApartmentNumber = model.ApartmentNumber;
                address.BuildingNumber = model.BuildingNumber;
                address.Street = model.Street;
                _uow.CustomerAddresses.Update(address);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> RemoveAddressAsync(int addressId)
        {
            try
            {
                var address = await _uow.CustomerAddresses.GetByIdAsync(addressId);
                if (address == null)
                    throw new ArgumentException("Address not found");
                // Check if the address is associated with any orders
                bool isAssociatedWithOrders = await _uow.Orders.GetAllAsIQueryable()
                    .AnyAsync(o => (o.BillingAddressId == addressId || o.ShippingAddressId == addressId) && o.ActualDeliveryDate == null);
                if (isAssociatedWithOrders)
                    throw new InvalidOperationException("Cannot delete address associated with orders.");
                _uow.CustomerAddresses.Delete(addressId);
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
