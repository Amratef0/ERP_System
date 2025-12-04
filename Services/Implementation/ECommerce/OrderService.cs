using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Services.Implementation.CRM;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Services.Interfaces.CRM;
using ERP_System_Project.Services.Interfaces.ECommerce;
using ERP_System_Project.UOW;
using ERP_System_Project.ViewModels;
using ERP_System_Project.ViewModels.ECommerce;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;

namespace ERP_System_Project.Services.Implementation.ECommerce
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string SessionKey = "MyCart";
        private readonly Dictionary<string, CartViewModel> _tempCarts = new();

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ICartService cartService,UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailSender)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        public async Task UpdateOrderStatusAsync(int orderId, int statusId)
        {
            // 1) جلب الأوردر
            var order = await _unitOfWork.Orders.GetAllAsIQueryable()
                .Include(o => o.Customer)
                .ThenInclude(c => c.ApplicationUser)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return;

            // 2) تحديث الحالة
            order.StatusId = statusId;
            order.ModifiedDate = DateTime.Now;
            await _unitOfWork.CompleteAsync();

            // 3) جلب اسم الحالة الجديدة
            var newStatus = await _unitOfWork.OrderStatusCodes.GetAllAsIQueryable()
                .Where(s => s.Id == statusId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            // 4) ارسال الايميل بصيغة HTML
            string email = order.Customer.ApplicationUser.Email;
            string subject = $"Order Status Updated - #{order.Id}";

            string htmlBody = $@"
<html>
<head>
  <style>
    body {{ font-family: Arial, sans-serif; color: #333; }}
    .header {{ background-color: #2196F3; color: white; padding: 10px; text-align: center; }}
    .content {{ padding: 20px; }}
    .status {{ font-size: 16px; font-weight: bold; color: #4CAF50; }}
    .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
  </style>
</head>
<body>
  <div class='header'>
    <h2>Order Status Update</h2>
  </div>

  <div class='content'>
    <p>Hi {order.Customer.FirstName},</p>
    <p>Your order <strong>#{order.Id}</strong> status has been updated to:</p>
    <p class='status'>{newStatus}</p>
    <p>Thank you for shopping with us!</p>
  </div>

  <div class='footer'>
    &copy; {DateTime.Now.Year} YourCompany. All rights reserved.<br/>
    Contact us: support@yourcompany.com
  </div>
</body>
</html>";

            await _emailSender.SendEmailAsync(email, subject, htmlBody);

        }



        // ===========================
        //     GET CUSTOMER ORDERS
        // ===========================
        public async Task<PageSourcePagination<MyOrdersVM>> GetCustomerOrdersAsync(string userId, int pageNumber, int pageSize)
        {
            // جلب العميل
            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
                throw new Exception("Customer not found");

            // جلب الأوردرات مع الـ pagination
            var customerOrders = await _unitOfWork.Orders
                .GetAllPaginatedEnhancedAsync(
                    filter: o => o.CustomerId == customer.Id,
                    orderBy: o => o.OrderByDescending(o => o.OrderDate), // أحدث أوردر فوق
                    selector: o => new MyOrdersVM
                    {
                        OrderId = o.Id,
                        BillingAddress = o.BillingAddress != null
                            ? $"{o.BillingAddress.ApartmentNumber} {o.BillingAddress.BuildingNumber} {o.BillingAddress.Street} | {o.BillingAddress.City} {o.BillingAddress.Country}"
                            : "NA",
                        OrderDate = o.OrderDate.ToLocalTime(), // تحويل DateTime عادي
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate.HasValue
                                                ? o.EstimatedDeliveryDate.Value.ToLocalTime()
                                                : (DateTime?)null, // تحويل DateTime? لو موجود
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
                            LineTotal = oi.LineTotal
                        }).ToList()
                    },
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    include: o => o.Include(o => o.Items).ThenInclude(oi => oi.Product)
                                  .Include(o => o.OrderStatusCode)
                                  .Include(o => o.PaymentMethodType)
                                  .Include(o => o.BillingAddress)
                );

            return customerOrders;
        }





        // ===========================
        //         MAKE ORDER
        // ===========================
        // OrderService - MakeOrderAsync
        public async Task MakeOrderAsync(string userId, CartViewModel cart, PaymentMethod paymentMethod)
        {
            if (cart == null || cart.productsCart.Count == 0)
                throw new Exception("Cart is empty");

            // 1) جلب العميل أو إنشاء واحد
            var customer = await _unitOfWork.Customers.GetAllAsIQueryable()
                .Include(c => c.CustomerAddresses)
                .Include(c => c.CustomerType)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
            {
                customer = new Customer
                {
                    ApplicationUserId = userId,
                    FirstName = "Default",
                    LastName = "Name",
                };
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();
            }

            // 2) التأكد من وجود BillingAddress
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

            // 3) إنشاء OrderItems
            var orderItems = new List<OrderItem>();
            foreach (var item in cart.productsCart)
            {
                var product = await _unitOfWork.Products.GetAllAsIQueryable()
                    .Include(p => p.Offer)
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null) continue;

                // تقليل الكمية
                product.Quantity -= item.Quantity;

                var offerDiscount = product.Offer?.DiscountPercentage ?? 0;
                var customerTypeDiscount = customer.CustomerType?.DiscountPercentage ?? 0;

                decimal totalDiscount = Math.Min((decimal)offerDiscount + (decimal)customerTypeDiscount, 100m);

                decimal discountPerUnit = product.StandardPrice * (totalDiscount / 100m);
                decimal discountedPrice = product.StandardPrice - discountPerUnit;
                decimal lineTotal = discountedPrice * item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.StandardPrice,
                    DiscountPercentage = totalDiscount,
                    DiscountAmount = discountPerUnit * item.Quantity,
                    LineTotal = lineTotal,
                    CreatedDate = DateTime.Now
                });
            }

            // 4) إنشاء الأوردر
            var order = new Order
            {
                CustomerId = customer.Id,
                BillingAddressId = billingAddress.Id,
                CurrencyId = 1,
                StatusId = (int)OrderStatus.Pending,
                PaymentMethodTypeId = paymentMethod == PaymentMethod.Cash ? (int)PaymentMethod.Cash : (int)PaymentMethod.Visa,
                OrderDate = DateTime.Now,
                EstimatedDeliveryDate = DateTime.Now.AddDays(1),
                ShippingAmount = 0,
                TaxAmount = 0,
                TotalAmount = orderItems.Sum(oi => oi.LineTotal),
                Items = orderItems
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            


            // 5) مسح الكارت
            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
            await _cartService.ClearCartAsync();
        }




        // ===========================
        //       CLEAR CART
        // ===========================
        private void ClearCartSession()
        {
            _httpContextAccessor.HttpContext.Session.Remove(SessionKey);
        }



        private async Task<string> GetCustomerAddressAsync(int customerId)
        {
            var customerAddress = await _unitOfWork.CustomerAddresses.GetAllAsIQueryable()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            return $"{customerAddress.ApartmentNumber} {customerAddress.BuildingNumber} {customerAddress.Street} | {customerAddress.City} {customerAddress.Country}";
        }



        public async Task<CartVM> GetCartVMAsync(string userId)
        {
            return await _cartService.GetAllFromCart();
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
        public async Task<List<OrderStatusCode>> GetOrderStatusListAsync()
        {
            return await _unitOfWork.OrderStatusCodes.GetAllAsIQueryable().ToListAsync();
        }
        public async Task<PageSourcePagination<MyOrdersVM>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var orders = await _unitOfWork.Orders
                .GetAllPaginatedEnhancedAsync(
                    filter: null, // كل الأوردرات
                    orderBy: o => o.OrderByDescending(o => o.OrderDate),
                    selector: o => new MyOrdersVM
                    {
                        OrderId = o.Id,
                        BillingAddress = o.BillingAddress != null
                            ? $"{o.BillingAddress.ApartmentNumber} {o.BillingAddress.BuildingNumber} {o.BillingAddress.Street} | {o.BillingAddress.City} {o.BillingAddress.Country}"
                            : "NA",
                        OrderDate = o.OrderDate.ToLocalTime(), // تحويل DateTime عادي
                        EstimatedDeliveryDate = o.EstimatedDeliveryDate.HasValue
                                                ? o.EstimatedDeliveryDate.Value.ToLocalTime()
                                                : (DateTime?)null, // تحويل DateTime? لو موجود
                        OrderStatus = o.OrderStatusCode.Name,
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
                            LineTotal = oi.LineTotal
                        }).ToList()
                    },
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    include: o => o.Include(o => o.Items).ThenInclude(oi => oi.Product)
                                  .Include(o => o.OrderStatusCode)
                                  .Include(o => o.PaymentMethodType)
                                  .Include(o => o.BillingAddress)
                );

            return orders;
        }


    }
}
