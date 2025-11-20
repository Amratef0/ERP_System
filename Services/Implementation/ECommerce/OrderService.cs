using ERP_System_Project.Models.CRM;
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
        private readonly Dictionary<string, CartViewModel> _tempCarts = new();
        private readonly ICartService _cartService;


        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
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



        public async Task MakeOrderAsync(string userId, CartViewModel cart)
        {
            if (cart == null || cart.productsCart.Count == 0)
                throw new Exception("Cart is empty");

            var productIDs = cart.productsCart.Select(p => p.ProductId).ToList();
            var productQuantities = cart.productsCart.Select(p => p.Quantity).ToList();
            var numberOfItems = productIDs.Count;

            // جلب العميل، لو مش موجود اعمل واحد جديد
            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
            {
                customer = new Customer
                {
                    ApplicationUserId = userId,
                    FirstName = "Default",
                    LastName = "Name"
                };
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();
            }

            // التأكد من وجود عنوان فواتير
            var billingAddress = customer.CustomerAddresses.FirstOrDefault();
            if (billingAddress == null)
            {
                billingAddress = new CustomerAddress
                {
                    CustomerId = customer.Id,
                    Street = "NA",
                    City = "NA",
                    Country = "NA"
                };
                customer.CustomerAddresses.Add(billingAddress);
                await _unitOfWork.CompleteAsync();
            }

            var orderItems = new List<OrderItem>();

            for (int i = 0; i < numberOfItems; i++)
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

                if (product == null) continue; // لو المنتج مش موجود تجاهله

                var discountAmount = product.Offer != null ?
                    (product.StandardPrice * productQuantities[i])
                    - (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                    : 0;

                var lineTotal = product.Offer != null ?
                    (product.StandardPrice - ((product.Offer.DiscountPercentage / 100m) * product.StandardPrice)) * productQuantities[i]
                    : product.StandardPrice * productQuantities[i];

                orderItems.Add(new OrderItem
                {
                    ProductId = productIDs[i],
                    Quantity = productQuantities[i],
                    CreatedDate = DateTime.Now,
                    UnitPrice = product.StandardPrice,
                    DiscountPercentage = product.Offer?.DiscountPercentage ?? 0,
                    DiscountAmount = discountAmount,
                    LineTotal = lineTotal
                });
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                BillingAddressId = billingAddress.Id,
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
        public async Task<CartVM> GetCartVMAsync(string userId)
        {
            // جلب السلة الحالية من الـ Session
            var cart = await _cartService.GetAllFromCart();
            return cart;
        }
        public async Task SaveCartForPayment(string userId, CartViewModel cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("PaymentCart_" + userId, JsonConvert.SerializeObject(cart));
        }

        public async Task<CartViewModel> GetCartFromPayment(string userId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var json = session.GetString("PaymentCart_" + userId);
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<CartViewModel>(json);
        }


    }
}
