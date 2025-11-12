using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string SessionKey = "MyCart";

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PageSourcePagination<MyOrdersVM>> GetCustomerOrdersAsync(string userId, int pageNumber, int pageSize)
        {
            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            var customerAddress = await GetCustomerAddressAsync(customer.Id);

            var customerOrders = await _unitOfWork.Orders
                .GetAllPaginatedEnhancedAsync(
                    filter: o => o.CustomerId == customer.Id,
                    orderBy: o => o.OrderByDescending(o => o.OrderDate),
                    selector: o => new MyOrdersVM
                    {
                        BillingAddress = customerAddress,
                        OrderDate = o.OrderDate,
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate,
                        OrderStatus = o.OrderStatusCode.Name,
                        PaymentMethodType = o.PaymentMethodType.Type,
                        ShippingAmount = o.ShippingAmount,
                        TaxAmount = o.TaxAmount,
                        TotalAmount = o.TotalAmount,

                        OrderItemsVMs = o.Items.Select(oi => new MyOrderItemsVM
                        {
                            ProductName = oi.Product.Name,
                            ProductImagePath = oi.Product.ImageURL,
                            DiscountAmount = oi.DiscountAmount,
                            DiscountPercentage = oi.DiscountPercentage,
                            UnitPrice = oi.UnitPrice,
                            Quantity = oi.Quantity,
                            LineTotal = oi.LineTotal,
                        } ).ToList()
                    },
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    include: o => 
                    o.Include(o => o.Items).ThenInclude(oi => oi.Product)
                    .Include(o => o.OrderStatusCode)
                    .Include(o => o.PaymentMethodType)
                );

            return customerOrders;
        }

        

        public async Task MakeOrderAsync(string userId)
        {
            var productDictionary = GetDictionaryFromSession(SessionKey);

            var productIDs = productDictionary.Keys.ToList();
            var productQuantities = productDictionary.Values.ToList();
            var numberOfItems = productIDs.Count;

            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId ==  userId);
            int customerId = customer!.Id;

            var orderItems = new List<OrderItem>();

            for(int i = 0; i < numberOfItems; i++)
            {
                await _unitOfWork.Products.GetAllAsIQueryable()
                   .Where(p => p.Id == productIDs[i])
                   .ExecuteUpdateAsync(p => p.SetProperty(
                       p => p.Quantity,
                       p => p.Quantity - productQuantities[i]));

                var product = _unitOfWork.Products
                    .GetAllAsIQueryable()
                    .Include(p => p.Offer)
                    .FirstOrDefault(p => p.Id == productIDs[i]);

                OrderItem orderItem = new OrderItem()
                {
                    ProductId = productIDs[i],
                    Quantity = productQuantities[i],
                    CreatedDate = DateTime.Now,
                    UnitPrice = product!.StandardPrice,
                    DiscountPercentage = product.Offer != null ? product.Offer.DiscountPercentage : 0,
                    DiscountAmount = product.Offer != null ?
                                    (product.StandardPrice * productQuantities[i])
                                    -
                                    (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                                    :0,
                    LineTotal =
                                   product.Offer != null ?
                                   (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                                   : product.StandardPrice * productQuantities[i],
                };
                orderItems.Add(orderItem);
            }

            Order order = new Order()
            {
                CustomerId = customerId,
                BillingAddressId = customer.CustomerAddresses.FirstOrDefault()!.Id,
                CurrencyId = 1,
                StatusId = (int)OrderStatus.Pending,
                PaymentMethodTypeId = (int)PaymentMethod.Cash,
                OrderDate = DateTime.Now,
                ShippingAmount = 0,
                TaxAmount = 0,
                EstimatedDeliveryDate = DateTime.Now.AddDays(1),
                TotalAmount = orderItems.Sum(oi => oi.LineTotal),
                Items = orderItems
            };
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            ClearCartSession(SessionKey);
        }

        private async Task<string> GetCustomerAddressAsync(int customerId)
        {
            var customerAddress = await _unitOfWork.CustomerAddresses.GetAllAsIQueryable()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            string addressFormat = $"{customerAddress.ApartmentNumber} {customerAddress.BuildingNumber} {customerAddress.Street} | {customerAddress.City} {customerAddress.Country}";
            return addressFormat;
        }
        private void ClearCartSession(string sessionKey)
           => _httpContextAccessor.HttpContext.Session.Remove(SessionKey);


        private Dictionary<int, int> GetDictionaryFromSession(string sessionKey)
        {
            var json = _httpContextAccessor
                .HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return new Dictionary<int, int>();

            return JsonConvert.DeserializeObject<Dictionary<int, int>>(json)
                ?? new Dictionary<int, int>(); ;
        }
    }
}
